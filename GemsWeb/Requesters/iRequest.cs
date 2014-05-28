using System;
using GemsWeb.Interfaces;

namespace GemsWeb.Requesters
{
    /// <summary>
    /// Represents an object that performs the downloading
    /// of an image.
    /// </summary>
    public interface iRequest
    {
        /// <summary>
        /// Attempts to make the request. Handles any unexpected
        /// exceptions internally.
        /// </summary>
        iResponse Get(Uri pUrl);
    }
}