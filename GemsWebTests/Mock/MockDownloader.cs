using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemsWeb;
using GemsWeb.Responses;

namespace GemsWebTests.Mock
{
    public class MockDownloader : iDownloader
    {
        /// <summary>
        /// Attempts to make the request. Handles any unexpected
        /// exceptions internally.
        /// </summary>
        public iResponse Get(Uri pUrl)
        {
            throw new NotImplementedException();
        }
    }
}
