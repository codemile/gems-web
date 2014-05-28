using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GemsWeb.Requesters
{
    public class GetBitmap : GetStream
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GetBitmap(string pUserAgent) 
            : base(pUserAgent)
        {
        }

        protected override ResponseContainer Read(Stream pStream)
        {
            using (Image img = Image.FromStream(pStream))
            {
                result = WebExceptionStatus.Success;
                _response = new ResponseContainer(response.StatusCode, response.ContentType, new System.Drawing.Bitmap(img));
            }
        }
    }
}
