using System;
using System.Net.Mime;
using GemsWeb.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GemsWebTests.Responses
{
    [TestClass]
    public class ResponseTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_1()
        {
            Response resp = new Response(null, new Object());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_2()
        {
            Response resp = new Response(new ContentType("text/plain"), null);
        }

        [TestMethod]
        public void Construct_3()
        {
            Response resp = new Response(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain",resp.getContentType().ToString());
            Assert.AreEqual("Hello",resp.getData());
            Assert.IsTrue(resp.isSuccess());
            Assert.AreEqual("text/plain",resp.getMessage());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_4()
        {
            Response resp = new Response(null);
        }

        [TestMethod]
        public void Construct_5()
        {
            Response resp = new Response(new Exception("Hello World"));
            Assert.IsFalse(resp.isSuccess());
            Assert.IsNotNull(resp.getException());
            Assert.AreEqual("Hello World",resp.getMessage());
        }

        [TestMethod]
        public void getContentType_1()
        {
            Response resp = new Response(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain", resp.getContentType().ToString());
        }

        [TestMethod]
        public void getData_1()
        {
            Response resp = new Response(new ContentType("text/plain"), "Hello");
            Assert.IsNotNull(resp.getData());
            Assert.AreEqual("Hello", resp.getData());
        }

        [TestMethod]
        public void getException_1()
        {
            Response resp = new Response(new Exception("Hello World"));
            Assert.IsNotNull(resp.getException());
            Assert.AreEqual("Hello World",resp.getException().Message);
        }

        [TestMethod]
        public void getMessage_1()
        {
            Response resp = new Response(new Exception("Hello World"));
            Assert.AreEqual("Hello World", resp.getMessage());
            resp = new Response(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain", resp.getMessage());
        }

        [TestMethod]
        public void isSuccess_1()
        {
            Response resp = new Response(new Exception("Hello World"));
            Assert.IsFalse(resp.isSuccess());
            resp = new Response(new ContentType("text/plain"), "Hello");
            Assert.IsTrue(resp.isSuccess());
        }
    }
}