using System;
using System.Net.Mime;
using GemsWeb.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GemsWebTests.Responses
{
    [TestClass]
    public class ResponseTests
    {
        private static Type _retester = typeof (ResponseOld);

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_1()
        {
            ResponseOld resp = new ResponseOld(null, new Object());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_2()
        {
            ResponseOld resp = new ResponseOld(new ContentType("text/plain"), null);
        }

        [TestMethod]
        public void Construct_3()
        {
            ResponseOld resp = new ResponseOld(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain",resp.getContentType().ToString());
            Assert.AreEqual("Hello",resp.getData());
            Assert.IsTrue(resp.isSuccess());
            Assert.AreEqual("text/plain",resp.getMessage());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Construct_4()
        {
            ResponseOld resp = new ResponseOld(null);
        }

        [TestMethod]
        public void Construct_5()
        {
            ResponseOld resp = new ResponseOld(new Exception("Hello World"));
            Assert.IsFalse(resp.isSuccess());
            Assert.IsNotNull(resp.getException());
            Assert.AreEqual("Hello World",resp.getMessage());
        }

        [TestMethod]
        public void getContentType_1()
        {
            ResponseOld resp = new ResponseOld(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain", resp.getContentType().ToString());
        }

        [TestMethod]
        public void getData_1()
        {
            ResponseOld resp = new ResponseOld(new ContentType("text/plain"), "Hello");
            Assert.IsNotNull(resp.getData());
            Assert.AreEqual("Hello", resp.getData());
        }

        [TestMethod]
        public void getException_1()
        {
            ResponseOld resp = new ResponseOld(new Exception("Hello World"));
            Assert.IsNotNull(resp.getException());
            Assert.AreEqual("Hello World",resp.getException().Message);
        }

        [TestMethod]
        public void getMessage_1()
        {
            ResponseOld resp = new ResponseOld(new Exception("Hello World"));
            Assert.AreEqual("Hello World", resp.getMessage());
            resp = new ResponseOld(new ContentType("text/plain"), "Hello");
            Assert.AreEqual("text/plain", resp.getMessage());
        }

        [TestMethod]
        public void isSuccess_1()
        {
            ResponseOld resp = new ResponseOld(new Exception("Hello World"));
            Assert.IsFalse(resp.isSuccess());
            resp = new ResponseOld(new ContentType("text/plain"), "Hello");
            Assert.IsTrue(resp.isSuccess());
        }
    }
}