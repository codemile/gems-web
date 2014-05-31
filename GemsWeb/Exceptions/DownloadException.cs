using System;

namespace GemsWeb.Exceptions
{
    public class DownloadException : Exception
    {
        public DownloadException(string pMessage)
            : base(pMessage)
        {
        }
    }
}