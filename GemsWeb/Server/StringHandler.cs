using System;
using System.Net;
using System.Text;
using GemsWeb.Annotations;

namespace GemsWeb.Server
{
    public class StringHandler : iResponseHandler
    {
        /// <summary>
        /// The text encoding
        /// </summary>
        private readonly Encoding _encoding;

        /// <summary>
        /// The string to send as a response.
        /// </summary>
        private readonly string _str;

        /// <summary>
        /// The content type.
        /// </summary>
        private readonly string _type;

        /// <summary>
        /// Constructor
        /// </summary>
        private StringHandler([NotNull] string pStr,
                              [NotNull] string pType,
                              [NotNull] Encoding pEncoding)
        {
            if (pStr == null)
            {
                throw new ArgumentNullException("pStr");
            }
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
            if (pEncoding == null)
            {
                throw new ArgumentNullException("pEncoding");
            }

            _str = pStr;
            _type = pType;
            _encoding = pEncoding;
        }

        /// <summary>
        /// Provides the string response.
        /// </summary>
        public void Handle(HttpListenerResponse pResponse)
        {
            pResponse.ContentType = "text/plain";
            pResponse.StatusCode = (int)HttpStatusCode.OK;

            byte[] buf = Encoding.UTF8.GetBytes(_str);
            pResponse.ContentEncoding = Encoding.UTF8;
            pResponse.ContentLength64 = buf.Length;
            pResponse.OutputStream.Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// Creates a HTML response.
        /// </summary>
        public static StringHandler Html(string pStr)
        {
            return new StringHandler(pStr, "text/html", Encoding.UTF8);
        }

        /// <summary>
        /// Creates a plain text response.
        /// </summary>
        public static StringHandler Text(string pStr)
        {
            return new StringHandler(pStr, "text/plain", Encoding.UTF8);
        }
    }
}