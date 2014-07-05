using System.IO;
using System.Net.Mime;
using System.Text;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    public class TextStreamReader : iStreamReader
    {
        private readonly iResponseOldFactory _oldFactory;

        public TextStreamReader(iResponseOldFactory pOldFactory)
        {
            _oldFactory = pOldFactory;
        }

        public iResponseOld Read(ContentType pContentType, Stream pStream)
        {
            using (StreamReader reader = new StreamReader(pStream, Encoding.GetEncoding(pContentType.CharSet)))
            {
                string str = reader.ReadToEnd();
                return _oldFactory.Create(pContentType, str);
            }
        }
    }
}