using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ValorAPI.Lib.Data.Endpoint.Content;
using ValorAPI.Lib.Data.Constant;

namespace ValorAPI.Lib.Connection.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void TestBuildUrl()
        {
            var region = Region.EUROPE;
            var contentsEndpoint = new ContentsEndpoint();
            var client = new Client(region, key: null);

            Assert.AreEqual(region, client.Region);
            Assert.AreEqual(client.BuildUrl(contentsEndpoint), "https://eu.api.riotgames.com/val/content/v1/contents");

            Assert.ThrowsException<ArgumentException>(() => client.BuildUrl(null));

            var nullRegionClient = new Client(null);
            Assert.ThrowsException<ArgumentException>(() => nullRegionClient.BuildUrl(contentsEndpoint));
        }
    }
}
