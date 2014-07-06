using System.Net;
using System.Text;

namespace GemsWeb.Server
{
    public class ErrorResponse : TextResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ErrorResponse()
            : base("An internal error has occurred.", "text/plain", HttpStatusCode.InternalServerError, Encoding.UTF8)
        {
        }
    }
}