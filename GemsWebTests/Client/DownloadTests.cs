using System;
using System.IO;
using System.Net;
using GemsLogger;
using GemsLogger.Formatters;
using GemsLogger.Writers;
using GemsWeb.Client;
using GemsWeb.Exceptions;
using GemsWeb.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReTester;

namespace GemsWebTests.Client
{
    [TestClass]
    public class DownloadTests
    {
        private static Type _retester = typeof (Download);
        private const string _AGENT = @"Mozilla/5.0 (Android; Mobile; rv:13.0) Gecko/13.0 Firefox/13.0";

        private Maker _test;

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

            _test = new Maker();
            _test.UseAs(uri);
            _test.UseAs(download);
            _test.UseAs<Stream>(stream);
        }

        [TestMethod]
        public void Construct_1()
        {
            ReTest.It("must not allow null parameters")
                .Throws<ArgumentNullException>()
                .That().Contains("pUserAgent").Then()
                .When(()=> { new Download(null, 5, 5000); });
        }

        [TestMethod]
        public void Construct_2()
        {
            ReTest.It("redirects must be >= 0")
                .Throws<ArgumentOutOfRangeException>()
                .That().Contains("pMaxRedirects").Then()
                .When(()=> { new Download(_AGENT, -1, 5000); });
        }

        [TestMethod]
        public void Construct_3()
        {
            ReTest.It("limit must be >= 0")
                .Throws<ArgumentOutOfRangeException>()
                .That().Contains("pLimit").Then()
                .When(()=> { new Download(_AGENT, 5, 0); });
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
        public void Get_2()
        {
            Download download = _test.Make<Download>();

            ReTest.It("none HTTP requests are not supported")
                .Throws<DownloadException>()
                .That()
                .Contains("HTTP scheme")
                .Then()
                .When(()=>download.Get(new Uri("ftp://localhost:9999")));
        }

        [TestMethod]
        public void Get_2b()
        {
            Download download = _test.Make<Download>();
            Uri uri = new Uri("http://localhost:9999/").MakeRelativeUri(new Uri("http://localhost:9999/home/page.html"));

            ReTest.It("Absolute URLs only")
                .Throws<DownloadException>()
                .That().Contains("Relative URLs").Then()
                .When(() => download.Get(uri));
        }

        [TestMethod]
        public void Get_3()
        {
            const string html = "<html>Hello world</html>";
            Download download = _test.Make<Download>();
            using (WebServer server = new WebServer(8888, TextResponse.Html(html)))
            {
                Response resp = ReTest.It("requests a HTML response")
                    .Returns<Response>(()=>download.Get(new Uri("http://localhost:8888/")));
                Assert.AreEqual(HttpStatusCode.OK, resp.Status);
                Assert.AreEqual("text/html", resp.ContentType);
                Assert.AreEqual(html, resp.getAsString());
            }
        }

        [TestMethod]
        public void Get_4()
        {
            Download download = _test.Make<Download>();
            using (WebServer server = new WebServer(8888, new NotFoundResponse()))
            {
                Response resp = ReTest.It("gives a 404 response")
                    .Returns<Response>(()=>download.Get(new Uri("http://localhost:8888/")));
                Assert.AreEqual(HttpStatusCode.NotFound, resp.Status);
            }
        }

        [TestMethod]
        public void Get_5()
        {
            Download download = _test.Make<Download>();
            using (WebServer server = new WebServer(8888, new ErrorResponse()))
            {
                Response resp = ReTest.It("gives a 500 response")
                    .Returns<Response>(()=>download.Get(new Uri("http://localhost:8888/")));
                Assert.AreEqual(HttpStatusCode.InternalServerError, resp.Status);
            }
        }

        [TestMethod]
        public void Get_6()
        {
            Download download = _test.Make<Download>();
            Response resp = download.Get(new Uri("http://localhost:8888/"));

            Assert.IsNull(resp);
            Assert.IsNotNull(download.LastError);
            Assert.IsInstanceOfType(download.LastError, typeof (WebException));
            Assert.IsTrue(download.LastError.Message.Contains("Unable to connect to the remote server"));
        }
    }
}