using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    /// <summary>
    /// Handles reading bitmaps from the input stream.
    /// </summary>
    public class BitmapReader : iStreamReader
    {
        /// <summary>
        /// Creates responses.
        /// </summary>
        private readonly iResponseOldFactory _responseOldFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public BitmapReader(iResponseOldFactory pResponseOldFactory)
        {
            if (pResponseOldFactory == null)
            {
                throw new NullReferenceException("Argument is null.");
            }
            _responseOldFactory = pResponseOldFactory;
        }

        /// <summary>
        /// Creates a bitmap from the input stream.
        /// </summary>
        public iResponseOld Read(ContentType pContentType, Stream pStream)
        {
            using (Image img = Image.FromStream(pStream))
            {
                return _responseOldFactory.Create(pContentType, new Bitmap(img));
            }
        }
    }
}