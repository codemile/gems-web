using System;
using System.Net.Mime;

namespace GemsWeb.Responses
{
    /// <summary>
    /// Holds the data from the response.
    /// </summary>
    [Obsolete]
    public class ResponseOld : iResponseOld
    {
        /// <summary>
        /// The content type.
        /// </summary>
        private readonly ContentType _contentType;

        /// <summary>
        /// The response data.
        /// </summary>
        private readonly object _data;

        /// <summary>
        /// The last exception.
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseOld(ContentType pContentType, object pData)
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
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseOld(Exception pException)
        {
            if (pException == null)
            {
                throw new NullReferenceException("pException is null.");
            }

            _contentType = null;
            _data = null;
            _exception = pException;
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
                ? _contentType.ToString()
                : _exception.Message;
        }

        /// <summary>
        /// True if the request was successful.
        /// </summary>
        public bool isSuccess()
        {
            return _exception == null && _data != null;
        }
    }
}