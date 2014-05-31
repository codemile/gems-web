using System;
using System.Net.Mime;

namespace GemsWeb.Responses
{
    public interface iResponseFactory
    {
        /// <summary>
        /// Creates a response that indicates an error.
        /// </summary>
        iResponse Create(Exception pError);

        /// <summary>
        /// Creates a response that indicates a success.
        /// </summary>
        iResponse Create(ContentType pContentType, object pData);
    }
}