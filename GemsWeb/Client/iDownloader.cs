using System;
using GemsWeb.Annotations;
using GemsWeb.Responses;

namespace GemsWeb.Client
{
    /// <summary>
    /// Represents an object that performs the downloading
    /// of an image.
    /// </summary>
    public interface iDownloader
    {
        /// <summary>
        /// Provides the last error that was caught by the downloader, or Null.
        /// </summary>
        Exception LastError { get; }

        /// <summary>
        /// Sends a GET request to the server and returns the response.
        /// </summary>
        /// <returns>Null if there was an error processing the request.</returns>
        [CanBeNull]
        Response Get(Uri pUrl);
    }
}