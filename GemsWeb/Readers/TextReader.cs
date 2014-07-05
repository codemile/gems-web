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
        private readonly iResponseOldFactory _responseOldFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextReader(iResponseOldFactory pResponseOldFactory)
        {
            if (pResponseOldFactory == null)
            {
                throw new NullReferenceException("Argument is null.");
            }
            _responseOldFactory = pResponseOldFactory;
        }

        /// <summary>
        /// Handles reading the text.
        /// </summary>
        public iResponseOld Read(ContentType pContentType, Stream pStream)
        {
            using (StreamReader reader = new StreamReader(pStream, Encoding.GetEncoding(pContentType.CharSet)))
            {
                return _responseOldFactory.Create(pContentType, reader.ReadToEnd());
            }
        }
    }
}