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
        private readonly iResponseFactory _responseFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamReaderFactory(iResponseFactory pResponseFactory)
        {
            _responseFactory = pResponseFactory;
        }

        /// <summary>
        /// Creates the reader.
        /// </summary>
        public iStreamReader Create(ContentType pContentType)
        {
            if (pContentType.MediaType.StartsWith("text/"))
            {
                return new TextReader(_responseFactory);
            }
            return pContentType.MediaType.StartsWith("image/")
                ? new BitmapReader(_responseFactory)
                : null;
        }
    }
}