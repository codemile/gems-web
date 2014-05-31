using System;
using System.Net.Mime;

namespace GemsWeb.Readers
{
    public class StreamReaderFactory : iStreamReaderFactory
    {
        public iStreamReader Create(ContentType pContentType)
        {
            throw new NotImplementedException();
        }
    }
}