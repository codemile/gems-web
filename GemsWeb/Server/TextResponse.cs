using System;
using System.Net;
using System.Text;
using GemsWeb.Annotations;

namespace GemsWeb.Server
{
    public class TextResponse : iResponseProvider
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
        /// The response code
        /// </summary>
        private readonly HttpStatusCode _code;

        /// <summary>
        /// Constructor
        /// </summary>
        protected TextResponse([NotNull] string pStr,
                              [NotNull] string pType,
                              HttpStatusCode pCode,
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
            _code = pCode;
            _encoding = pEncoding;
        }

        /// <summary>
        /// Provides the string response.
        /// </summary>
        public void Handle(HttpListenerResponse pResponse)
        {
            pResponse.ContentType = _type;
            pResponse.StatusCode = (int)_code;
            pResponse.StatusDescription = string.Format("{0} {1}", (int)_code, _code);
            pResponse.ProtocolVersion = new Version("1.1");

            byte[] buf = Encoding.UTF8.GetBytes(_str);

            pResponse.ContentEncoding = _encoding;
            pResponse.ContentLength64 = buf.Length;
            pResponse.OutputStream.Write(buf, 0, buf.Length);

        }

        /// <summary>
        /// Creates a HTML response.
        /// </summary>
        public static TextResponse Html(string pStr)
        {
            return new TextResponse(pStr, "text/html", HttpStatusCode.OK, Encoding.UTF8);
        }

        /// <summary>
        /// Creates a plain text response.
        /// </summary>
        public static TextResponse Text(string pStr)
        {
            return new TextResponse(pStr, "text/plain", HttpStatusCode.OK, Encoding.UTF8);
        }
    }
}