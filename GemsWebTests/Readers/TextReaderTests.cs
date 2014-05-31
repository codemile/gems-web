using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using GemsWeb.Responses;
using GemsWebTests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextReader = GemsWeb.Readers.TextReader;

namespace GemsWebTests.Readers
{
    [TestClass]
    public class TextReaderTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void Construct_1()
        {
            TextReader reader = new TextReader(null);
        }

        [TestMethod]
        public void Construct_2()
        {
            TextReader reader = new TextReader(new MockResponseFactory());
        }

        [TestMethod]
        public void Read_1()
        {
            const string str = "Hello World";
            TextReader reader = new TextReader(new MockResponseFactory());
            iResponse resp = reader.Read(new ContentType("text/plain; charset=UTF-8"), new MemoryStream(Encoding.UTF8.GetBytes(str)));
            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.isSuccess());
            Assert.AreEqual(str,resp.getData().ToString());
        }
    }
}