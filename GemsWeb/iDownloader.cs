using System;
using GemsWeb.Responses;

namespace GemsWeb
{
    /// <summary>
    /// Represents an object that performs the downloading
    /// of an image.
    /// </summary>
    public interface iDownloader
    {
        /// <summary>
        /// Attempts to make the request. Handles any unexpected
        /// exceptions internally.
        /// </summary>
        iResponse Get(Uri pUrl);
    }
}