using System;
using System.Net.Mime;

namespace GemsWeb.Responses
{
    [Obsolete]
    public interface iResponseOldFactory
    {
        /// <summary>
        /// Creates a response that indicates an error.
        /// </summary>
        iResponseOld Create(Exception pError);

        /// <summary>
        /// Creates a response that indicates a success.
        /// </summary>
        iResponseOld Create(ContentType pContentType, object pData);

        /// <summary>
        /// Creates a response that doesn't contain any body.
        /// </summary>
        iResponseOld Empty();
    }
}