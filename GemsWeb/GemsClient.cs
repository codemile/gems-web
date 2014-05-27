using System;
using System.Diagnostics;
using System.Net;

namespace GemsWeb
{
    /// <summary>
    /// Overrides the WebClient to allow control over some settings.
    /// </summary>
    public class GemsClient : WebClient
    {
        /// <summary>
        /// The request timeout.
        /// </summary>
        private readonly int _timeout;

        /// <summary>
        /// The user agent for requests.
        /// </summary>
        private readonly string _userAgent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTimeout">Timeout in seconds</param>
        /// <param name="pUserAgent">The user agent</param>
        public GemsClient(int pTimeout, string pUserAgent)
        {
            _userAgent = pUserAgent;
            _timeout = 1000 * pTimeout;

            // Bug fix: Stops strange characters from appearing in articles.
            Encoding = System.Text.Encoding.UTF8;
        }

        /// <summary>
        /// Set the request timeout.
        /// </summary>
        protected override WebRequest GetWebRequest(Uri pAddress)
        {
            HttpWebRequest request = base.GetWebRequest(pAddress) as HttpWebRequest;
            Debug.Assert(request != null, "request != null");

            request.Timeout = _timeout;
            request.UserAgent = _userAgent;

            return request;
        }
    }
}