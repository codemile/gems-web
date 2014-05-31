using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    /// <summary>
    /// Handles reading text from the input stream.
    /// </summary>
    public class TextReader : iStreamReader
    {
        /// <summary>
        /// Creates the response object.
        /// </summary>
        private readonly iResponseFactory _responseFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextReader(iResponseFactory pResponseFactory)
        {
            if (pResponseFactory == null)
            {
                throw new NullReferenceException("Argument is null.");
            }
            _responseFactory = pResponseFactory;
        }

        /// <summary>
        /// Handles reading the text.
        /// </summary>
        public iResponse Read(ContentType pContentType, Stream pStream)
        {
            using (StreamReader reader = new StreamReader(pStream, Encoding.GetEncoding(pContentType.CharSet)))
            {
                return _responseFactory.Create(pContentType, reader.ReadToEnd());
            }
        }
    }
}