using System;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWebTests.Mock.Responses
{
    public class MockResponseOld : iResponseOld
    {
        private readonly ContentType _contentType;
        private readonly object _data;
        private readonly Exception _exception;
        private readonly bool _success;

        public MockResponseOld(ContentType pContentType, object pData, Exception pException, bool pSuccess)
        {
            _contentType = pContentType;
            _data = pData;
            _exception = pException;
            _success = pSuccess;
        }

        /// <summary>
        /// If the response has data, then this is the content type for that data.
        /// </summary>
        public ContentType getContentType()
        {
            return _contentType;
        }

        /// <summary>
        /// Gets the RAW data object, or Null if there is no data.
        /// </summary>
        public object getData()
        {
            return _data;
        }

        /// <summary>
        /// A reference to an exception that are caught making the request.
        /// </summary>
        public Exception getException()
        {
            return _exception;
        }

        /// <summary>
        /// A message describes this response. Usually the error message.
        /// </summary>
        public string getMessage()
        {
            return _exception == null
                ? (_contentType == null ? "error" : _contentType.ToString())
                : _exception.Message;
        }

        /// <summary>
        /// True if the request was successful.
        /// </summary>
        public bool isSuccess()
        {
            return _success;
        }
    }
}