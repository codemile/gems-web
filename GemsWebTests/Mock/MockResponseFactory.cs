using System;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWebTests.Mock
{
    public class MockResponseFactory : iResponseFactory
    {
        /// <summary>
        /// Creates a response that indicates an error.
        /// </summary>
        public iResponse Create(Exception pError)
        {
            return new MockResponse(null, null, pError, false);
        }

        /// <summary>
        /// Creates a response that indicates a success.
        /// </summary>
        public iResponse Create(ContentType pContentType, object pData)
        {
            return new MockResponse(pContentType, pData, null, true);
        }
    }
}