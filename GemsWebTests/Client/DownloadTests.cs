using System;
using System.IO;
using GemsWeb.Client;
using GemsWeb.Readers;
using GemsWeb.Responses;
using GemsWeb.Server;
using GemsWebTests.Mock.Readers;
using GemsWebTests.Mock.Responses;
using Logging;
using Logging.Formatters;
using Logging.Writers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester;

namespace GemsWebTests.Client
{
    [TestClass]
    public class DownloadTests
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof(DownloadTests));

        private static Type _retester = typeof (Download);
        private const string _AGENT = @"Mozilla/5.0 (Android; Mobile; rv:13.0) Gecko/13.0 Firefox/13.0";

        private Maker _test;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void AtStart()
        {
            Logger.LogDetails = true;
            Logger.LogFinest = true;
            Logger.LogDebug = true;
            Logger.Close();
            Logger.Add(new FormatWriter(new ConsoleWriter(true), new SimpleFormat()));

            Uri uri = new Uri("http://www.thinkingmedia.ca");
            Download download = new Download(_AGENT, 5, 500000);
            MemoryStream stream = new MemoryStream();

            // mock objects
            MockStreamReaderFactory streamReaderFactory = new MockStreamReaderFactory();
            MockResponseOldFactory responseOldFactory = new MockResponseOldFactory();

            _test = new Maker();
            _test.UseAs(uri);
            _test.UseAs(download);
            _test.UseAs<Stream>(stream);

            _test.UseAs<iStreamReaderFactory>(streamReaderFactory);
            _test.UseAs<iResponseOldFactory>(responseOldFactory);
        }

        [TestMethod]
        public void Construct_1()
        {
            ReTest.It("must not allow null parameters")
                .Throws<ArgumentNullException>()
                .That().Contains("pUserAgent").Then()
                .When(() => { new Download(null, 5, 5000); });
        }

        [TestMethod]
        public void Construct_2()
        {
            ReTest.It("redirects must be >= 0")
                .Throws<ArgumentOutOfRangeException>()
                .That().Contains("pMaxRedirects").Then()
                .When(() => { new Download(_AGENT, -1, 5000); });
        }

        [TestMethod]
        public void Construct_3()
        {
            ReTest.It("limit must be >= 0")
                .Throws<ArgumentOutOfRangeException>()
                .That().Contains("pLimit").Then()
                .When(() => { new Download(_AGENT, 5, 0); });
        }

        [TestMethod]
        public void Get_1()
        {
            Download download = _test.Make<Download>();

            ReTest.It("throws null exception for Get")
                .Throws<ArgumentNullException>()
                .That().Contains("pUrl").Then()
                .When(()=> { download.Get(null); });
        }

        [TestMethod]
        [DeploymentItem(@"Data\www.thinkingmedia.ca.html")]
        public void Get_2()
        {
            Download download = _test.Make<Download>();

            using (WebServer server = new WebServer(8888))
            {
                Response resp = ReTest.It("creates a response object")
                    .Returns<Response>(() => download.Get(new Uri("http://localhost:8888/")));

                //Assert.IsTrue(resp.isSuccess());
                //Assert.AreEqual("text/html", resp.getContentType().MediaType);
                //Assert.AreEqual("UTF-8", resp.getContentType().CharSet);
            }
        }
    }
}