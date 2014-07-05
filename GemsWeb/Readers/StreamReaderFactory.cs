using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    /// <summary>
    /// Creates a reader based upon the content type.
    /// </summary>
    public class StreamReaderFactory : iStreamReaderFactory
    {
        /// <summary>
        /// Used by the readers.
        /// </summary>
        private readonly iResponseOldFactory _responseOldFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamReaderFactory(iResponseOldFactory pResponseOldFactory)
        {
            _responseOldFactory = pResponseOldFactory;
        }

        /// <summary>
        /// Creates the reader.
        /// </summary>
        public iStreamReader Create(ContentType pContentType)
        {
            if (pContentType.MediaType.StartsWith("text/"))
            {
                return new TextReader(_responseOldFactory);
            }
            return pContentType.MediaType.StartsWith("image/")
                ? new BitmapReader(_responseOldFactory)
                : null;
        }
    }
}