using System;
using System.Net.Mime;

namespace GemsWeb.Responses
{
    public interface iResponse
    {
        /// <summary>
        /// If the response has data, then this is the content type for that data.
        /// </summary>
        ContentType getContentType();

        /// <summary>
        /// Gets the RAW data object, or Null if there is no data.
        /// </summary>
        object getData();

        /// <summary>
        /// A reference to an exception that are caught making the request.
        /// </summary>
        Exception getException();

        /// <summary>
        /// A message describes this response. Usually the error message.
        /// </summary>
        string getMessage();

        /// <summary>
        /// True if the request was successful.
        /// </summary>
        bool isSuccess();
    }
}