using System.Drawing;
using System.Net;

namespace GemsWeb.Interfaces
{
    public interface iResponse
    {
        WebExceptionStatus getStatus();

        /// <summary>
        /// This is the content type of the response.
        /// </summary>
        string getContentType();

        /// <summary>
        /// If the request was successful. This is the
        /// status code of the response.
        /// </summary>
        HttpStatusCode getStatusCode();

        /// <summary>
        /// This reads the image.
        /// </summary>
        Bitmap ReadBitmap();
    }
}