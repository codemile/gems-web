using System;
using System.Net.Mime;

namespace GemsWeb.Responses
{
    /// <summary>
    /// Holds the data from the response.
    /// </summary>
    public class Response : iResponse
    {
        private readonly ContentType _contentType;
        private readonly object _data;
        private readonly Exception _exception;
        private readonly bool _success;

        public Response(ContentType pContentType, object pData)
        {
            if (pContentType == null)
            {
                throw new NullReferenceException("pContentType is null.");
            }
            if (pData == null)
            {
                throw new NullReferenceException("pData is null.");
            }

            _contentType = pContentType;
            _data = pData;
            _exception = null;
            _success = true;
        }

        public Response(Exception pException)
        {
            _contentType = null;
            _data = null;
            _exception = pException;
            _success = false;
        }

        /// <summary>
        /// The context type.
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