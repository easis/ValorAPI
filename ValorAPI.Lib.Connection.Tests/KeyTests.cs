using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Connection.Tests
{
    [TestClass]
    public class KeyTests
    {
        public const string VALID_KEY = "RGAPI-00000000-1111-2222-3333-444444444444";

        [TestMethod]
        public void TestIsValid()
        {
            Assert.IsTrue(Key.IsValid(VALID_KEY));
            Assert.IsFalse(Key.IsValid("RGAPI-g0000000-1111-2222-3333-444444444444"));
            Assert.IsFalse(Key.IsValid("TEST-00000000-1111-2222-3333-444444444444"));

            Assert.ThrowsException<ArgumentException>(() => Key.IsValid(null));
            Assert.ThrowsException<ArgumentException>(() => Key.IsValid(""));
        }
    }
}
