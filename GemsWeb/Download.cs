using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using GemsWeb.Annotations;
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
        public Download([NotNull] iStreamReaderFactory pStreamReaderFactory,
                        [NotNull] iResponseFactory pResponseFactory,
                        [NotNull] string pUserAgent,
                        int pMaxRedirects)
        {
            if (pStreamReaderFactory == null)
            {
                throw new ArgumentNullException("pStreamReaderFactory");
            }
            if (pResponseFactory == null)
            {
                throw new ArgumentNullException("pResponseFactory");
            }
            if (pUserAgent == null)
            {
                throw new ArgumentNullException("pUserAgent");
            }
            if (pMaxRedirects < 0)
            {
                throw new ArgumentOutOfRangeException("pMaxRedirects");
            }

            _streamReaderFactory = pStreamReaderFactory;
            _responseFactory = pResponseFactory;
            _userAgent = pUserAgent;
            _maxRedirects = pMaxRedirects;
        }

        /// <summary>
        /// Makes the request.
        /// </summary>
        [NotNull]
        public iResponse Get([NotNull] Uri pUrl)
        {
            if (pUrl == null)
            {
                throw new ArgumentNullException("pUrl");
            }

            WebRequest request = WebRequest.Create(pUrl);
            HttpWebRequest httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.AllowAutoRedirect = true;
                httpRequest.MaximumAutomaticRedirections = _maxRedirects;
                httpRequest.UserAgent = _userAgent;
            }

            try
            {
                using (Stream stream = request.GetRequestStream())
                {
                    try
                    {
                        ContentType contentType = new ContentType(request.ContentType);
                        iStreamReader reader = _streamReaderFactory.Create(contentType);
                        if (reader == null)
                        {
                            throw new DownloadException(string.Format("Content-type: [{0}] has no reader.", contentType));
                        }

                        return reader.Read(contentType, stream);
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
    }
}