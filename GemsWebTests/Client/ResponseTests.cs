using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using GemsWeb.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester;

namespace GemsWebTests.Client
{
    [TestClass]
    public class ResponseTests
    {
        private static Type _retester = typeof (Response);
        private const string _HELLO_WORLD = "Hello World!";
        private Maker _test;

        /// <summary>
        /// Converts a string to bytes using an encoding method, or UTF-8 if the encoding method could not be found.
        /// </summary>
        private static byte[] ToBytes(string pEncoding, string pStr)
        {
            byte[] bytes;
            try
            {
                bytes = Encoding.GetEncoding(pEncoding).GetBytes(pStr);
            }
            catch (ArgumentException)
            {
                bytes = Encoding.UTF8.GetBytes(pStr);
            }
            return bytes;
        }

        /// <summary>
        /// Creates a Response object containing a string that uses an encoding.
        /// </summary>
        private Response CreateString(string pStr, string pEncoding = "UTF-8")
        {
            Response resp = _test.Make<Response>("OK");
            resp.Encoding = pEncoding;
            byte[] bytes = ToBytes(pEncoding, pStr);
            resp.Read(new MemoryStream(bytes), 1000);
            return resp;
        }

        [TestInitialize]
        public void AStartUp()
        {
            Uri uri = new Uri("http://localhost:8888/");

            _test = new Maker();
            _test.UseAs(uri);
            _test.HowToMake("OK",
                ()=>new Response(uri, uri, HttpStatusCode.OK) {ContentType = "text/plain", Encoding = "UTF-8"});
            _test.HowToMake("Image",
                ()=>new Response(uri, uri, HttpStatusCode.OK) {ContentType = "image/png"});
        }

        [TestMethod]
        public void Construct_1()
        {
            ReTest.It("can not accept null parameters")
                .Throws<ArgumentNullException>()
                .That().Contains("pOriginalUrl").Then()
                .When(()=>new Response(null, _test.Make<Uri>(), HttpStatusCode.OK));

            ReTest.It("can not accept null parameters")
                .Throws<ArgumentNullException>()
                .That().Contains("pUrl").Then()
                .When(()=>new Response(_test.Make<Uri>(), null, HttpStatusCode.OK));

            ReTest.It("can not accept null parameters")
                .Throws<ArgumentNullException>()
                .When(()=>new Response(null, null, HttpStatusCode.OK));
        }

        [TestMethod]
        public void Read_1()
        {
            Response resp = _test.Make<Response>("OK");
            ReTest.It("can not accept null stream")
                .Throws<ArgumentNullException>()
                .That().Contains("pInput").Then()
                .When(()=>resp.Read(null, 5000));
        }

        [TestMethod]
        public void Read_2()
        {
            Response resp = _test.Make<Response>("OK");
            ReTest.It("limit must be > 0")
                .Throws<ArgumentOutOfRangeException>()
                .That().Contains("pLimit").Then()
                .When(()=>resp.Read(new MemoryStream(), -1));
        }

        [TestMethod]
        public void Read_3()
        {
            // this tests truncating the input by a max length
            const long limit = 50000;
            byte[] bytes = new byte[limit * 5];
            Response resp = _test.Make<Response>("OK");
            long num = resp.Read(new MemoryStream(bytes), limit);

            Assert.AreEqual(limit, num, "must be the same");
            Assert.IsTrue(resp.Truncated, "must be truncated");
            Assert.IsNotNull(resp.Data, "there must be data");
            Assert.AreEqual(limit, resp.Data.Length);
            Assert.AreEqual(limit, resp.Length);
        }

        [TestMethod]
        public void getAsBitmap_1()
        {
            Response resp = _test.Make<Response>("OK");

            ReTest.It("throws an exception if there is no data")
                .Throws<InvalidOperationException>()
                .That().Contains("contains no data").Then()
                .When(()=>resp.getAsBitmap());
        }

        [TestMethod]
        public void getAsBitmap_2()
        {
            Response resp = CreateString(_HELLO_WORLD);
            Assert.AreEqual(_HELLO_WORLD, resp.getAsString());
            Assert.IsNull(resp.getAsBitmap(), "is null if not a bitmap");
        }

        [TestMethod]
        public void getAsBitmap_3()
        {
            foreach (string img in new[] {"PNG", "GIF", "JPEG"})
            {
                Response resp = _test.Make<Response>("Image");
                resp.ContentType = string.Format("image/{0}", img);

                string path = string.Format("GemsWebTests.Data.image.{0}", img.ToLower());
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
                Assert.IsNotNull(stream,string.Format("is {0} embedded?", path));

                long read = resp.Read(stream, 0);
                using (Bitmap bitmap = resp.getAsBitmap())
                {
                    Assert.IsNotNull(bitmap);
                    Assert.AreEqual(256,bitmap.Width);
                    Assert.AreEqual(345,bitmap.Height);
                }

                Assert.IsTrue(read > 0);
            }
        }

        [TestMethod]
        public void getAsString_1()
        {
            Response resp = _test.Make<Response>("OK");
            ReTest.It("throws an exception if there is no data")
                .Throws<InvalidOperationException>()
                .That().Contains("contains no data").Then()
                .When(()=>resp.getAsString());
        }

        [TestMethod]
        public void getAsString_2()
        {
            Response resp = _test.Make<Response>("OK");
            resp.Read(new MemoryStream(), 1000);
            Assert.AreEqual("", resp.getAsString(), "empty stream creates empty string");
        }

        [TestMethod]
        public void getAsString_3()
        {
            Response resp = CreateString(_HELLO_WORLD);

            Assert.AreEqual("text/plain", resp.ContentType);
            Assert.AreEqual("UTF-8", resp.Encoding);
            Assert.AreEqual(_HELLO_WORLD, resp.getAsString());
        }

        [TestMethod]
        public void getAsString_4()
        {
            Response resp = CreateString(_HELLO_WORLD, "Unicode");

            Assert.AreEqual("text/plain", resp.ContentType);
            Assert.AreEqual("Unicode", resp.Encoding);
            Assert.AreEqual(_HELLO_WORLD, resp.getAsString(), "can handle Unicode encoding");
        }

        [TestMethod]
        public void getAsString_5()
        {
            Response resp = CreateString(_HELLO_WORLD, "Magic");

            Assert.AreEqual("text/plain", resp.ContentType);
            Assert.AreEqual("Magic", resp.Encoding);
            Assert.IsNull(resp.getAsString(), "null for unknown encoding types");
        }
    }
}