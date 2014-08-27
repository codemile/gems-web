using System;
using System.IO;
using System.Net;
using GemsWeb.Annotations;
using GemsWeb.Exceptions;
using Logging;

namespace GemsWeb.Client
{
    /// <summary>
    /// Performs the request to the remote server for the file.
    /// </summary>
    public class Download : iDownloader
    {
        /// <summary>
        /// 1 kilobyte
        /// </summary>
        public const int KB = 1024;

        /// <summary>
        /// 1 megabyte
        /// </summary>
        public const int MB = KB * 1024;

        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (Download));

        /// <summary>
        /// The max size for a response.
        /// </summary>
        private readonly long _limit;

        /// <summary>
        /// Max number of redirects.
        /// </summary>
        private readonly int _maxRedirects;

        /// <summary>
        /// The user agent to use for requests.
        /// </summary>
        private readonly string _userAgent;

        /// <summary>
        /// Creates a response object from a HTTP response.
        /// </summary>
        private Response ProcessResponse([NotNull] Uri pUrl, [NotNull] HttpWebResponse pHttpResp)
        {
            Response resp = new Response(pUrl, pHttpResp.ResponseUri, pHttpResp.StatusCode)
                            {
                                ContentType = pHttpResp.ContentType,
                                Encoding = pHttpResp.ContentEncoding,
                                CharSet = pHttpResp.CharacterSet
                            };

            if (resp.Status != HttpStatusCode.OK)
            {
                _logger.Finer("{0} - {1}", (int)resp.Status, resp.ContentType);
            }

/*
            if (pHttpResp.ContentLength <= 0)
            {
                return resp;
            }
*/

            using (Stream stream = pHttpResp.GetResponseStream())
            {
                if (stream != null)
                {
                    resp.Read(stream, _limit);
                }
            }

            return resp;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Download([NotNull] string pUserAgent,
                        int pMaxRedirects,
                        long pLimit)
        {
            if (pUserAgent == null)
            {
                throw new ArgumentNullException("pUserAgent");
            }
            if (pMaxRedirects < 0)
            {
                throw new ArgumentOutOfRangeException("pMaxRedirects");
            }
            if (pLimit <= 0)
            {
                throw new ArgumentOutOfRangeException("pLimit");
            }

            _userAgent = pUserAgent;
            _maxRedirects = pMaxRedirects;
            _limit = pLimit;
        }

        /// <summary>
        /// Provides the last error that was caught by the downloader, or Null.
        /// </summary>
        public Exception LastError { get; private set; }

        /// <summary>
        /// Sends a GET request to the server and returns the response.
        /// </summary>
        /// <returns>Null if there was an error processing the request.</returns>
        public Response Get([NotNull] Uri pUrl)
        {
            LastError = null;

            if (pUrl == null)
            {
                throw new ArgumentNullException("pUrl");
            }

            if (!pUrl.IsAbsoluteUri)
            {
                throw new DownloadException(string.Format("Relative URLs not supported: {0}", pUrl));
            }

            if (pUrl.Scheme != "http"
                && pUrl.Scheme != "https")
            {
                throw new DownloadException(string.Format("Only HTTP scheme allowed: {0}", pUrl));
            }

            _logger.Finer(string.Format("GET: {0}", pUrl));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = _maxRedirects;
            request.UserAgent = _userAgent;
            request.Method = "GET";

            try
            {
                using (HttpWebResponse httpResp = (HttpWebResponse)request.GetResponse())
                {
                    return ProcessResponse(pUrl, httpResp);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    return ProcessResponse(pUrl, (HttpWebResponse)ex.Response);
                }

                LastError = ex;
                _logger.Exception(ex);
            }
            catch (Exception ex)
            {
                // TODO: What other kinds of exceptions are possible?
                LastError = ex;
                _logger.Exception(ex);
            }

            return null;
        }
    }
}