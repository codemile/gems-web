using System;
using System.IO;
using System.Net;
using GemsWeb.Annotations;

namespace GemsWeb.Client
{
    /// <summary>
    /// Contains the response from the server.
    /// </summary>
    public sealed class Response
    {
        /// <summary>
        /// The character set used in the response.
        /// </summary>
        [CanBeNull]
        public string CharSet { get; set; }

        /// <summary>
        /// The content type of the response.
        /// </summary>
        [CanBeNull]
        public string ContentType { get; set; }

        /// <summary>
        /// The body of the response.
        /// </summary>
        [CanBeNull]
        public byte[] Data { get; private set; }

        /// <summary>
        /// The character encoding used.
        /// </summary>
        [CanBeNull]
        public string Encoding { get; set; }

        /// <summary>
        /// The length of the body.
        /// </summary>
        public long Length {
            get { return (Data == null) ? 0 : Data.Length; }
        }

        /// <summary>
        /// True if the some of the data was trimmed because of download limits.
        /// </summary>
        public bool Truncated { get; private set; }

        /// <summary>
        /// The original URL before any redirects.
        /// </summary>
        public Uri OriginalUrl { get; private set; }

        /// <summary>
        /// The response status code.
        /// </summary>
        public HttpStatusCode Status { get; private set; }

        /// <summary>
        /// URL that represents this response.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pOriginalUrl">The original URL request</param>
        /// <param name="pUrl">The URL of the response</param>
        /// <param name="pStatus">The status code of the response.</param>
        public Response([NotNull] Uri pOriginalUrl, [NotNull] Uri pUrl, HttpStatusCode pStatus)
        {
            if (pOriginalUrl == null)
            {
                throw new ArgumentNullException("pOriginalUrl");
            }

            OriginalUrl = pOriginalUrl;
            Url = pUrl;
            Status = pStatus;
            Truncated = false;
            Data = null;
        }

        /// <summary>
        /// Reads the contents of the stream to the attached Data property.
        /// </summary>
        /// <param name="pInput">The stream to read.</param>
        /// <param name="pLimit">The cut off limit for the stream size.</param>
        public void Read([NotNull] Stream pInput, long pLimit)
        {
            if (pInput == null)
            {
                throw new ArgumentNullException("pInput");
            }

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = pInput.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);

                    if (ms.Length >= pLimit)
                    {
                        Truncated = true;
                        break;
                    }
                }
                Data = ms.ToArray();
            }
        }
    }
}