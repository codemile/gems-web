using System;
using System.IO;
using System.Net;
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
        private Maker _test;

        [TestInitialize]
        public void AStartUp()
        {
            Uri uri = new Uri("http://localhost:8888/");

            _test = new Maker();
            _test.UseAs(uri);
            _test.HowToMake("OK",
                ()=>new Response(uri, uri, HttpStatusCode.OK) {ContentType = "text/plain", Encoding = "UTF-8"});
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
        public void getAsString_1()
        {
            Response resp = _test.Make<Response>("OK");
            Assert.IsNull(resp.getAsString(), "null when no data");
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
            const string str = "Hello World!";
            Response resp = _test.Make<Response>("OK");
            resp.Read(new MemoryStream(Encoding.UTF8.GetBytes(str)), 1000);

            Assert.AreEqual("text/plain", resp.ContentType);
            Assert.AreEqual("UTF-8", resp.Encoding);
            Assert.AreEqual(str, resp.getAsString());
        }

        [TestMethod]
        public void getAsString_4()
        {
            const string str = "Hello World!";
            Response resp = _test.Make<Response>("OK");
            resp.Encoding = "Unicode";
            resp.Read(new MemoryStream(Encoding.Unicode.GetBytes(str)), 1000);

            Assert.AreEqual("text/plain", resp.ContentType);
            Assert.AreEqual("Unicode", resp.Encoding);
            Assert.AreEqual(str, resp.getAsString(), "can handle Unicode encoding");
        }

        [TestMethod]
        public void getAsString_5()
        {
            const string str = "Hello World!";
            Response resp = _test.Make<Response>("OK");
            resp.Encoding = "Magic";
            resp.Read(new MemoryStream(Encoding.UTF8.GetBytes(str)), 1000);

            Assert.AreEqual("Magic", resp.Encoding);
            Assert.IsNull(resp.getAsString(), "null for unknown encoding types");
        }
    }
}