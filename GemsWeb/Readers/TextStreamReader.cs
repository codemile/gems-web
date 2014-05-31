using System.IO;
using System.Net.Mime;
using System.Text;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    public class TextStreamReader : iStreamReader
    {
        private readonly iResponseFactory _factory;

        public TextStreamReader(iResponseFactory pFactory)
        {
            _factory = pFactory;
        }

        public iResponse Read(ContentType pContentType, long pLength, Stream pStream)
        {
            using (StreamReader reader = new StreamReader(pStream, Encoding.GetEncoding(pContentType.CharSet)))
            {
                string str = reader.ReadToEnd();
                return _factory.Create(pContentType, str);
            }
        }
    }
}