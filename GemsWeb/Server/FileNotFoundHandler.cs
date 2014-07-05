using System.Net;

namespace GemsWeb.Server
{
    public class FileNotFoundHandler : iResponseHandler
    {
        /// <summary>
        /// Sets the 404 status.
        /// </summary>
        public void Handle(HttpListenerResponse pResponse)
        {
            pResponse.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}