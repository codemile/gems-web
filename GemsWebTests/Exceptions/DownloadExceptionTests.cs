using System;
using GemsWeb.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GemsWebTests.Exceptions
{
    [TestClass]
    public class DownloadExceptionTests
    {
        private static Type _retester = typeof (DownloadException);

        [TestMethod]
        public void Construct_1()
        {
            DownloadException ex = new DownloadException("Hello world");
            Assert.AreEqual(ex.Message, "Hello world");
        }
    }
}
