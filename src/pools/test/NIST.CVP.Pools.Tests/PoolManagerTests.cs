using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolManagerTests
    {
        private string _testPath;
        private string _configFile = "testConfig.json";
        private string _fullPath;

        [SetUp]
        public void SetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
            _fullPath = Path.Combine(_testPath, _configFile);
        }

        [Test]
        public void ShouldLoadConfigCorrectly()
        {
            var poolManager = new PoolManager(_fullPath);

            Assert.AreEqual(2, poolManager._shaPools.Count);

            var shaPools = poolManager._shaPools;
            Assert.AreEqual(ModeValues.SHA2, shaPools[0].WaterType.HashFunction.Mode);
            Assert.AreEqual(DigestSizes.d256, shaPools[0].WaterType.HashFunction.DigestSize);

            Assert.AreEqual(1, poolManager._aesPools.Count);
            Assert.AreEqual("encrypt", poolManager._aesPools[0].WaterType.Direction);
        }
    }
}
