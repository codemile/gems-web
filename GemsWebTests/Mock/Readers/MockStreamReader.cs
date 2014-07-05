using System.IO;
using System.Net.Mime;
using GemsWeb.Readers;
using GemsWeb.Responses;

namespace GemsWebTests.Mock.Readers
{
    public class MockStreamReader : iStreamReader
    {
        private readonly iResponseOld _responseOld;

        public MockStreamReader(iResponseOld pResponseOld)
        {
            _responseOld = pResponseOld;
        }

        public iResponseOld Read(ContentType pContentType, Stream pStream)
        {
            return _responseOld;
        }
    }
}