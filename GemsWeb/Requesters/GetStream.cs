using System;
using System.IO;
using System.Net;
using GemsWeb.Interfaces;
using Logging;

namespace GemsWeb.Requesters
{
    /// <summary>
    /// Performs the request to the remote server for the file.
    /// </summary>
    public abstract class GetStream : iRequest
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (GetStream));

        /// <summary>
        /// The user agent to use for requests.
        /// </summary>
        private readonly string _userAgent;

        /// <summary>
        /// Constructor
        /// </summary>
        protected GetStream(string pUserAgent)
        {
            _userAgent = pUserAgent;
        }

        protected abstract ResponseContainer Read(Stream pStream);

        /// <summary>
        /// Makes the request.
        /// </summary>
        public iResponse Get(Uri pUrl)
        {
            WebExceptionStatus result = WebExceptionStatus.UnknownError;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);

                // follow redirects
                request.AllowAutoRedirect = true;
                request.MaximumAutomaticRedirections = 3;
                request.UserAgent = _userAgent;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream inputStream = response.GetResponseStream())
                {
                    // TODO: Check if this is right. Maybe the caller needs to dispose of the image.
                    if (inputStream != null)
                    {
                        return Read(inputStream);
                    }
                }
            }
            catch (WebException e)
            {
                _logger.Error(pUrl.ToString());
                _logger.Error(e.Message);
                result = e.Status;
            }
            catch (Exception e)
            {
                _logger.Error(pUrl.ToString());
                _logger.Error(e.Message);
            }

            return null;
        }
    }
}