using System.Net;
using System.Text;

namespace GemsWeb.Server
{
    public class NotFoundResponse : TextResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotFoundResponse()
            : base("The URL you requested could not be found.", "text/plain", HttpStatusCode.NotFound, Encoding.UTF8)
        {
        }
    }
}