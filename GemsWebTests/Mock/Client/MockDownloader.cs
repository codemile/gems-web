using System;
using GemsWeb.Client;

namespace GemsWebTests.Mock.Client
{
    public class MockDownloader : iDownloader
    {
        /// <summary>
        /// Sends a GET request to the server and returns the response.
        /// </summary>
        /// <returns>Null if there was an error processing the request.</returns>
        Response iDownloader.Get(Uri pUrl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides the last error that was caught by the downloader, or Null.
        /// </summary>
        public Exception LastError { get; private set; }
    }
}