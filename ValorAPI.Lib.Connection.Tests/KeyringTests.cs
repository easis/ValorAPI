using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ValorAPI.Lib.Data.Endpoint.Content;
using ValorAPI.Lib.Connection.Exception;
using ValorAPI.Lib.Data.Constant;
using ValorAPI.Lib.Data.Exception;

namespace ValorAPI.Lib.Connection.Tests
{
    [TestClass]
    public class KeyringTests
    {
        private Key EuKey = new Key(Region.EUROPE, KeyTests.VALID_KEY);
        private Key BrKey = new Key(Region.BRAZIL, KeyTests.VALID_KEY);
        private Key KrKey = new Key(Region.KOREA, KeyTests.VALID_KEY);
        private Key NaKey = new Key(Region.NORTH_AMERICA, KeyTests.VALID_KEY);

        [TestMethod]
        public void TestConstructor()
        {
            var validInstance = new Keyring();
            Assert.IsNotNull(validInstance);
            Assert.IsInstanceOfType(validInstance, typeof(Keyring));

            var validInstanceWithSingleKey = new Keyring();
            Assert.IsInstanceOfType(validInstanceWithSingleKey, typeof(Keyring));

            var validInstanceWithSingleParamKey = new Keyring(this.EuKey);
            Assert.IsInstanceOfType(validInstanceWithSingleParamKey, typeof(Keyring));

            var validInstanceWithMultipleKeys = new Keyring(this.BrKey, this.KrKey);
            Assert.IsInstanceOfType(validInstanceWithMultipleKeys, typeof(Keyring));

            Assert.ThrowsException<ArgumentException>(() => new Keyring(null));
        }

        [TestMethod]
        public void TestAddKey()
        {
            var keyring = new Keyring();
            Assert.ThrowsException<ArgumentException>(() => keyring.AddKey(null));
            Assert.ThrowsException<ArgumentException>(() => keyring.AddKey(null, null));
            Assert.ThrowsException<ArgumentException>(() => keyring.AddKey(Region.EUROPE, null));
            Assert.ThrowsException<ArgumentException>(() => keyring.AddKey(null, KeyTests.VALID_KEY));
            Assert.ThrowsException<InvalidApiKeyException>(() => keyring.AddKey(Region.EUROPE, "invalid"));

            Assert.IsTrue(keyring.AddKey(EuKey));
            Assert.IsFalse(keyring.AddKey(EuKey));
        }

        [TestMethod]
        public void TestGetKeysFromRegion()
        {
            var emptyKeyring = new Keyring();

            Assert.ThrowsException<ArgumentException>(() => emptyKeyring.GetKeysFromRegion(null));
            Assert.ThrowsException<RegionNotFoundException>(() => emptyKeyring.GetKeysFromRegion(new Region(null, null)));

            var euKeyring = new Keyring(this.EuKey);
            var euKeys = euKeyring.GetKeysFromRegion(Region.EUROPE);
            Assert.IsInstanceOfType(euKeys, typeof(List<Key>));
            Assert.AreEqual(euKeys.Count, 1);
        }
    }
}
