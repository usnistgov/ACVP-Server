using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
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

        [Test]
        public void ShouldAddToPoolsCorrectly()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "encrypt",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Result = new AesResult
                {
                    CipherText = new BitString("BEEFFACE"),
                    Iv = new BitString("BEEFFACE20"),
                    Key = new BitString("BEEFFACE40"),
                    PlainText = new BitString("BEEFFACE80")
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.AddResultToPool(paramHolder);

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldNotAddBadValuesToPool()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "bad value",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Result = new AesResult
                {
                    CipherText = new BitString("BEEFFACE"),
                    Iv = new BitString("BEEFFACE20"),
                    Key = new BitString("BEEFFACE40"),
                    PlainText = new BitString("BEEFFACE80")
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.AddResultToPool(paramHolder);

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldGetResultFromPool()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "encrypt",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.IsFalse(result.PoolEmpty);
        }

        [Test]
        public void ShouldGetBadResultWhenPoolDoesNotExist()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "bad value",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Result);
            Assert.IsTrue(result.PoolEmpty);
        }

        [Test]
        public void ShouldGetPoolStatus()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "encrypt",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.GetPoolStatus(paramHolder);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(default(int), result.FillLevel);
            Assert.IsTrue(result.PoolExists);
        }

        [Test]
        public void ShouldReturnBadStatusWhenPoolDoesNotExist()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var paramHolder = new ParameterHolder
            {
                Parameters = new AesParameters
                {
                    DataLength = 128,
                    Direction = "bad value",
                    KeyLength = 128,
                    Mode = BlockCipherModesOfOperation.Ecb
                },
                Type = PoolTypes.AES
            };

            var result = poolManager.GetPoolStatus(paramHolder);

            Assert.IsNotNull(result);
            Assert.AreEqual(default(int), result.FillLevel);
            Assert.IsFalse(result.PoolExists);
        }

        [Test]
        public void ShouldSavePools()
        {
            var poolManager = new PoolManager(_fullPath, _testPath);

            var result = poolManager.SavePools();

            Assert.IsTrue(result);
        }
    }
}
