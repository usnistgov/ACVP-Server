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
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolManagerTests
    {
        private readonly Mock<IOptions<PoolConfig>> _mockOptionsPoolConfig = new Mock<IOptions<PoolConfig>>();
        private readonly PoolConfig _poolConfig = new PoolConfig()
        {
            Port = 42,
            RootUrl = "localhost",
            ShouldRecyclePoolWater = false
        };
        private string _testPath;
        private string _configFile = "testConfig.json";
        private string _fullPath;
        private PoolManager _subject;

        [SetUp]
        public void SetUp()
        {
            _mockOptionsPoolConfig.Setup(s => s.Value).Returns(_poolConfig);
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
            _fullPath = Path.Combine(_testPath, _configFile);
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _fullPath, _testPath);
        }

        [Test]
        public void ShouldLoadConfigCorrectly()
        {
            Assert.AreEqual(
                2, 
                _subject.Pools.Count(w => w.ParamType == typeof(ShaParameters)),
                "Sha pool count"
            );

            var shaPools = _subject.Pools;
            Assert.AreEqual(ModeValues.SHA2, ((ShaParameters)shaPools[0].Param).HashFunction.Mode);
            Assert.AreEqual(DigestSizes.d256, ((ShaParameters)shaPools[0].Param).HashFunction.DigestSize);

            Assert.AreEqual(
                1, 
                _subject.Pools.Count(w => w.ParamType == typeof(AesParameters)),
                "Aes pool count");
            Assert.AreEqual("encrypt", ((AesParameters)_subject.Pools[2].Param).Direction);
        }

        [Test]
        public void ShouldAddToPoolsCorrectly()
        {
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

            var result = _subject.AddResultToPool(paramHolder);

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldNotAddBadValuesToPool()
        {
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

            var result = _subject.AddResultToPool(paramHolder);

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldGetResultFromPool()
        {
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

            var result = _subject.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.IsFalse(result.PoolEmpty);
        }

        [Test]
        public void ShouldGetBadResultWhenPoolDoesNotExist()
        {
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

            var result = _subject.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Result);
            Assert.IsTrue(result.PoolEmpty);
        }

        [Test]
        public void ShouldGetPoolStatus()
        {
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

            var result = _subject.GetPoolStatus(paramHolder);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(default(int), result.FillLevel);
            Assert.IsTrue(result.PoolExists);
        }

        [Test]
        public void ShouldReturnBadStatusWhenPoolDoesNotExist()
        {
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

            var result = _subject.GetPoolStatus(paramHolder);

            Assert.IsNotNull(result);
            Assert.AreEqual(default(int), result.FillLevel);
            Assert.IsFalse(result.PoolExists);
        }

        [Test]
        public void ShouldSavePools()
        {
            var result = _subject.SavePools();

            Assert.IsTrue(result);
        }

        [Test]
        public void ShouldGetPoolConfigInfo()
        {
            var individualPools = _subject.Pools.Count;

            var result = _subject.GetPoolProperties();

            Assert.AreEqual(individualPools, result.Count);
        }

        [Test]
        public void ShouldSetNewPoolConfig()
        {
            var prechangeConfig = _subject.GetPoolProperties().First();
            var newConfig = new PoolProperties()
            {
                FilePath = prechangeConfig.FilePath,
                MaxCapacity = prechangeConfig.MaxCapacity,
                MaxWaterReuse = prechangeConfig.MaxWaterReuse + 42,
                MonitorFrequency = prechangeConfig.MonitorFrequency,
                PoolType = prechangeConfig.PoolType
            };

            _subject.EditPoolProperties(newConfig);

            var result = _subject.GetPoolProperties().First();

            Assert.AreEqual(newConfig.MaxWaterReuse, result.MaxWaterReuse);
        }

        [Test]
        public void ShouldProperlySaveConfigurationFile()
        {
            var fullPath = Path.Combine(_testPath, "saveChangesConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, fullPath, _testPath);

            // Change the pool configuration
            var prechangeConfig = _subject.GetPoolProperties().First();
            // copy of object as to not work with the original reference
            var prechangeConfigCopy = new PoolProperties()
            {
                FilePath = prechangeConfig.FilePath,
                MaxCapacity = prechangeConfig.MaxCapacity,
                MaxWaterReuse = prechangeConfig.MaxWaterReuse,
                MonitorFrequency = prechangeConfig.MonitorFrequency,
                PoolType = prechangeConfig.PoolType
            };
            var newConfig = new PoolProperties()
            {
                FilePath = prechangeConfigCopy.FilePath,
                MaxCapacity = prechangeConfigCopy.MaxCapacity,
                MaxWaterReuse = prechangeConfigCopy.MaxWaterReuse + 42,
                MonitorFrequency = prechangeConfigCopy.MonitorFrequency,
                PoolType = prechangeConfigCopy.PoolType
            };
            _subject.EditPoolProperties(newConfig);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, fullPath, _testPath);
            var postChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(newConfig.MaxWaterReuse, postChangeConfig.MaxWaterReuse, "config change");

            // Change the value back
            _subject.EditPoolProperties(prechangeConfigCopy);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, fullPath, _testPath);
            var validateOriginalChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(prechangeConfigCopy.MaxWaterReuse, validateOriginalChangeConfig.MaxWaterReuse, "original file check");
        }
    }
}
