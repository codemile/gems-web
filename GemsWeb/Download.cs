using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using GemsWeb.Exceptions;
using GemsWeb.Readers;
using GemsWeb.Responses;

namespace GemsWeb
{
    /// <summary>
    /// Performs the request to the remote server for the file.
    /// </summary>
    public class Download : iDownloader
    {
        /// <summary>
        /// Max number of redirects.
        /// </summary>
        private readonly int _maxRedirects;

        /// <summary>
        /// Creates response objects.
        /// </summary>
        private readonly iResponseFactory _responseFactory;

        /// <summary>
        /// Creates readers based upon the content type of the response.
        /// </summary>
        private readonly iStreamReaderFactory _streamReaderFactory;

        /// <summary>
        /// The user agent to use for requests.
        /// </summary>
        private readonly string _userAgent;

        /// <summary>
        /// Constructor
        /// </summary>
        public Download(iStreamReaderFactory pStreamReaderFactory, iResponseFactory pResponseFactory, string pUserAgent,
                        int pMaxRedirects = 3)
        {
            if (pStreamReaderFactory == null)
            {
                throw new NullReferenceException("pStreamReaderFactory is null.");
            }
            if (pResponseFactory == null)
            {
                throw new NullReferenceException("pResponseFactory is null.");
            }
            if (string.IsNullOrWhiteSpace(pUserAgent))
            {
                throw new NullReferenceException("pUserAgent can not be null or empty.");
            }
            if (pMaxRedirects <= 0)
            {
                throw new ArgumentOutOfRangeException("pMaxRedirects", "Must be greater then 0.");
            }

            _streamReaderFactory = pStreamReaderFactory;
            _responseFactory = pResponseFactory;
            _userAgent = pUserAgent;
            _maxRedirects = pMaxRedirects;
        }

        /// <summary>
        /// Makes the request.
        /// </summary>
        public iResponse Get(Uri pUrl)
        {
            if (pUrl == null)
            {
                throw new NullReferenceException("pUrl can not be null.");
            }

            try
            {
                WebRequest request = CreateRequest(pUrl);
                WebResponse resp = request.GetResponse();
                using (Stream stream = resp.GetResponseStream())
                {
                    if (stream == null)
                    {
                        throw new NullReferenceException("Unable to open response stream.");
                    }
                    try
                    {
                        ContentType contentType = new ContentType(resp.ContentType);
                        iStreamReader reader = _streamReaderFactory.Create(contentType);
                        if (reader == null)
                        {
                            throw new DownloadException(string.Format("Content-type: [{0}] has no reader.", contentType));
                        }
                        iResponse response = reader.Read(contentType, resp.ContentLength, stream);
                        return response;
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            catch (Exception e)
            {
                return _responseFactory.Create(e);
            }
        }

        /// <summary>
        /// Creates a request object.
        /// </summary>
        public WebRequest CreateRequest(Uri pUrl)
        {
            if (pUrl == null)
            {
                throw new NullReferenceException("pUrl can not be null.");
            }
            if (!pUrl.IsAbsoluteUri)
            {
                throw new ArgumentException("Only absolute URIs allowed.", "pUrl");
            }

            WebRequest request = WebRequest.Create(pUrl);

            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                // follow redirects
                httpRequest.AllowAutoRedirect = true;
                httpRequest.MaximumAutomaticRedirections = _maxRedirects;
                httpRequest.UserAgent = _userAgent;
            }

            return request;
        }
    }
}