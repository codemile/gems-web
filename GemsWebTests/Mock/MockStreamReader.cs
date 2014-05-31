using System.IO;
using System.Net.Mime;
using GemsWeb.Readers;
using GemsWeb.Responses;

namespace GemsWebTests.Mock
{
    public class MockStreamReader : iStreamReader
    {
        private readonly iResponse _response;

        public MockStreamReader(iResponse pResponse)
        {
            _response = pResponse;
        }

        public iResponse Read(ContentType pContentType, Stream pStream)
        {
            return _response;
        }
    }
}