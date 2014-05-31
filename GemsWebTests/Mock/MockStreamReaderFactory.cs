using System.Net.Mime;
using GemsWeb.Readers;

namespace GemsWebTests.Mock
{
    public class MockStreamReaderFactory : iStreamReaderFactory
    {
        private readonly iStreamReader _reader;

        public MockStreamReaderFactory()
        {
            _reader = null;
        }

        public MockStreamReaderFactory(iStreamReader pReader)
        {
            _reader = pReader;
        }

        public iStreamReader Create(ContentType pContentType)
        {
            return _reader;
        }

        public static MockStreamReaderFactory Create(string pContentType, object pData)
        {
            return
                new MockStreamReaderFactory(
                    new MockStreamReader(new MockResponse(new ContentType(pContentType), pData, null, true)));
        }
    }
}