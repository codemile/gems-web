using System;
using System.IO;
using GemsWeb;
using GemsWeb.Readers;
using GemsWeb.Responses;
using GemsWebTests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester;

namespace GemsWebTests
{
    [TestClass]
    public class DownloadTests
    {
        private static Type _retester = typeof (Download);
        private const string _AGENT = @"Mozilla/5.0 (Android; Mobile; rv:13.0) Gecko/13.0 Firefox/13.0";

        private Maker _test;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void AtStart()
        {
            Uri uri = new Uri("http://www.thinkingmedia.ca");
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), _AGENT, 5);
            MemoryStream stream = new MemoryStream();

            // mock objects
            MockStreamReaderFactory streamReaderFactory = new MockStreamReaderFactory();
            MockResponseFactory responseFactory = new MockResponseFactory();

            _test = new Maker();
            _test.UseAs(uri);
            _test.UseAs(download);
            _test.UseAs<Stream>(stream);

            _test.UseAs<iStreamReaderFactory>(streamReaderFactory);
            _test.UseAs<iResponseFactory>(responseFactory);
        }

        [TestMethod]
        public void Construct_1()
        {
            iStreamReaderFactory streamReaderFactory = _test.Make<iStreamReaderFactory>();
            iResponseFactory responseFactory = _test.Make<iResponseFactory>();

            ReTest.It("must not allow null parameters")
                .Throws<ArgumentNullException>()
                .When(()=> { new Download(null, null, null, 5); })
                .When(()=> { new Download(streamReaderFactory, null, null, 5); })
                .When(()=> { new Download(null, responseFactory, null, 5); })
                .When(()=> { new Download(null, null, _AGENT, 5); });
        }

        [TestMethod]
        public void Construct_2()
        {
            iStreamReaderFactory streamReaderFactory = _test.Make<iStreamReaderFactory>();
            iResponseFactory responseFactory = _test.Make<iResponseFactory>();

            ReTest.It("redirects must be >= 0")
                .Throws<ArgumentOutOfRangeException>()
                .When(()=> { new Download(streamReaderFactory, responseFactory, _AGENT, -1); });
        }

        [TestMethod]
        public void Get_1()
        {
            Download download = _test.Make<Download>();

            ReTest.It("throws null exception for Get")
                .Throws<ArgumentNullException>()
                .When(()=> { download.Get(null); });
        }

        [TestMethod]
        [DeploymentItem(@"Data\www.thinkingmedia.ca.html")]
        public void Get_2()
        {
            Download download = _test.Make<Download>();

            string path = "file:///" + TestContext.DeploymentDirectory + "/www.thinkingmedia.ca.html";
            path = path.Replace(@"\", "/");

            iResponse resp = ReTest.It("creates a mock response")
                .Returns<MockResponse>(()=>download.Get(new Uri(path)));

            Assert.IsTrue(resp.isSuccess());
            Assert.AreEqual("text/html", resp.getContentType().MediaType);
            Assert.AreEqual("UTF-8", resp.getContentType().CharSet);
        }
    }
}