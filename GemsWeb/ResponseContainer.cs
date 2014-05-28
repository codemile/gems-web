using System.Drawing;
using System.Net;
using GemsWeb.Interfaces;

namespace GemsWeb
{
    /// <summary>
    /// Holds the data from the response.
    /// </summary>
    public class ResponseContainer : iResponse
    {
        /// <summary>
        /// The image.
        /// </summary>
        private Bitmap _bitmap { get; set; }

        /// <summary>
        /// The status code.
        /// </summary>
        private HttpStatusCode _code { get; set; }

        /// <summary>
        /// The content type.
        /// </summary>
        private string _contentType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseContainer(HttpStatusCode pCode, string pContextType, Bitmap pBitmap)
        {
            _code = pCode;
            _contentType = pContextType;
            _bitmap = pBitmap;
        }

        /// <summary>
        /// The status code
        /// </summary>
        public HttpStatusCode getStatusCode()
        {
            return _code;
        }

        public WebExceptionStatus getStatus()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The context type.
        /// </summary>
        public string getContentType()
        {
            return _contentType;
        }

        /// <summary>
        /// The image.
        /// </summary>
        public Bitmap ReadBitmap()
        {
            return _bitmap;
        }
    }
}