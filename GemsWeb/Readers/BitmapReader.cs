using System.Drawing;
using System.IO;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    public class BitmapReader : iStreamReader
    {
        public iResponse Read(ContentType pContentType, long pLength, Stream pStream)
        {
            using (Image img = Image.FromStream(pStream))
            {
                Bitmap bm = new Bitmap(img);
                //return new Response(0, null, new Bitmap(img));
                return null;
            }
        }
    }
}