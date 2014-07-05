using System;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWebTests.Mock.Responses
{
    public class MockResponseOldFactory : iResponseOldFactory
    {
        /// <summary>
        /// Creates a response that indicates an error.
        /// </summary>
        public iResponseOld Create(Exception pError)
        {
            return new MockResponseOld(null, null, pError, false);
        }

        /// <summary>
        /// Creates a response that indicates a success.
        /// </summary>
        public iResponseOld Create(ContentType pContentType, object pData)
        {
            return new MockResponseOld(pContentType, pData, null, true);
        }

        /// <summary>
        /// Creates a response that doesn't contain any body.
        /// </summary>
        public iResponseOld Empty()
        {
            return new MockResponseOld(null, null, null, false);
        }
    }
}