using System;
using System.Net;
using GemsWeb;
using GemsWeb.Responses;
using GemsWebTests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GemsWebTests
{
    [TestClass]
    public class DownloadTests
    {
        private const string _AGENT = @"Mozilla/5.0 (Android; Mobile; rv:13.0) Gecko/13.0 Firefox/13.0";

        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            Download download = new Download(null, new MockResponseFactory(), _AGENT, 5);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_2()
        {
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), null, 5);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void Construct_3()
        {
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), _AGENT, 0);
        }

        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void CreateRequest_1()
        {
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), _AGENT);
            WebRequest webRequest = download.CreateRequest(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void CreateRequest_2()
        {
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), _AGENT);
            download.CreateRequest(new Uri("/", UriKind.Relative));
        }

        [TestMethod]
        public void CreateRequest_3()
        {
            Download download = new Download(new MockStreamReaderFactory(), new MockResponseFactory(), _AGENT, 5);
            WebRequest request = download.CreateRequest(new Uri("http://www.cnn.com"));
            Assert.IsNotNull(request);

            HttpWebRequest httpRequest = request as HttpWebRequest;
            Assert.IsNotNull(httpRequest);
            Assert.IsTrue(httpRequest.AllowAutoRedirect);
            Assert.AreEqual(_AGENT, httpRequest.UserAgent);
            Assert.AreEqual("http://www.cnn.com/", httpRequest.RequestUri.ToString());
            Assert.AreEqual(5, httpRequest.MaximumAutomaticRedirections);
        }

        [TestMethod]
        [DeploymentItem(@"Data\www.thinkingmedia.ca.html")]
        public void Get_1()
        {
            Download download = new Download(
                MockStreamReaderFactory.Create("text/html; charset=UTF-8", "<html></html>"),
                new MockResponseFactory(),
                _AGENT, 5);

            string path = "file:///" + TestContext.DeploymentDirectory + "/www.thinkingmedia.ca.html";
            path = path.Replace(@"\", "/");

            iResponse resp = download.Get(new Uri(path));
            Assert.IsNotNull(resp);
            Assert.IsInstanceOfType(resp, typeof (MockResponse));
            Assert.IsTrue(resp.isSuccess(), resp.getMessage());
            Assert.AreEqual("text/html", resp.getContentType().MediaType);
            Assert.AreEqual("UTF-8", resp.getContentType().CharSet);
        }
    }
}