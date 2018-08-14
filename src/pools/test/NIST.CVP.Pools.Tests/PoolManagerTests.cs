using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;
using System.Linq;

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
            var poolManager = new PoolManager(_fullPath, _testPath);

            Assert.AreEqual(
                2, 
                poolManager.Pools.Count(w => w.ParamType == typeof(ShaParameters)),
                "Sha pool count"
            );

            var shaPools = poolManager.Pools;
            Assert.AreEqual(ModeValues.SHA2, ((ShaParameters)shaPools[0].Param).HashFunction.Mode);
            Assert.AreEqual(DigestSizes.d256, ((ShaParameters)shaPools[0].Param).HashFunction.DigestSize);

            Assert.AreEqual(
                1, 
                poolManager.Pools.Count(w => w.ParamType == typeof(AesParameters)),
                "Aes pool count");
            Assert.AreEqual("encrypt", ((AesParameters)poolManager.Pools[2].Param).Direction);
        }
    }
}
