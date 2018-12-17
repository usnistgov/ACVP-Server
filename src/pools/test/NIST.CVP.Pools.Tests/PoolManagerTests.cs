using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolManagerTests
    {
        private readonly Mock<IOptions<PoolConfig>> _mockOptionsPoolConfig = new Mock<IOptions<PoolConfig>>();
        private readonly Mock<IOracle> _mockOracle = new Mock<IOracle>();
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
        public async Task SetUp()
        {
            _mockOptionsPoolConfig.Setup(s => s.Value).Returns(_poolConfig);
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
            _fullPath = Path.Combine(_testPath, _configFile);
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, _fullPath, _testPath);
            await _subject.LoadPools();
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
                Type = PoolTypes.AES,
                Result = new AesResult()
                {
                    CipherText = new BitString("01"),
                    Iv = new BitString("02"),
                    Key = new BitString("03"),
                    PlainText = new BitString("04")
                }
            };

            _subject.AddResultToPool(paramHolder);
            var result = _subject.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.IsFalse(result.PoolTooEmpty);
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
                Type = PoolTypes.AES,
                Result = new AesResult()
                {
                    CipherText = new BitString("01"),
                    Iv = new BitString("02"),
                    Key = new BitString("03"),
                    PlainText = new BitString("04")
                }
            };

            var result = _subject.GetResultFromPool(paramHolder);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Result);
            Assert.IsTrue(result.PoolTooEmpty);
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
                Type = PoolTypes.AES,
                Result = new AesResult()
                {
                    CipherText = new BitString("01"),
                    Iv = new BitString("02"),
                    Key = new BitString("03"),
                    PlainText = new BitString("04")
                }
            };

            _subject.AddResultToPool(paramHolder);
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
                MinCapacity = prechangeConfig.MinCapacity + 1,
                RecycleRatePerHundred = prechangeConfig.RecycleRatePerHundred,
                PoolType = prechangeConfig.PoolType
            };

            _subject.EditPoolProperties(newConfig);

            var result = _subject.GetPoolProperties().First();

            Assert.AreEqual(newConfig.MinCapacity, result.MinCapacity);
        }

        [Test]
        public async Task ShouldProperlySaveConfigurationFile()
        {
            var fullPath = Path.Combine(_testPath, "saveChangesConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();

            // Change the pool configuration
            var prechangeConfig = _subject.GetPoolProperties().First();
            // copy of object as to not work with the original reference
            var prechangeConfigCopy = new PoolProperties()
            {
                FilePath = prechangeConfig.FilePath,
                MaxCapacity = prechangeConfig.MaxCapacity,
                MinCapacity = prechangeConfig.MinCapacity,
                RecycleRatePerHundred = prechangeConfig.RecycleRatePerHundred,
                PoolType = prechangeConfig.PoolType
            };
            var newConfig = new PoolProperties()
            {
                FilePath = prechangeConfigCopy.FilePath,
                MaxCapacity = prechangeConfigCopy.MaxCapacity,
                MinCapacity = prechangeConfigCopy.MinCapacity + 42,
                RecycleRatePerHundred = prechangeConfigCopy.RecycleRatePerHundred,
                PoolType = prechangeConfigCopy.PoolType
            };
            _subject.EditPoolProperties(newConfig);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();
            
            var postChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(newConfig.MinCapacity, postChangeConfig.MinCapacity, "config change");

            // Change the value back
            _subject.EditPoolProperties(prechangeConfigCopy);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();
            
            var validateOriginalChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(prechangeConfigCopy.MinCapacity, validateOriginalChangeConfig.MinCapacity, "original file check");
        }

        [Test]
        public async Task ShouldFillPoolWhenNotAtCapacity()
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();

            var waterCount = _subject.Pools.Sum(s => s.WaterLevel);

            Assert.IsTrue(waterCount == 0, "Expecting empty pools");

            var result = await _subject.SpawnJobForMostShallowPool(1);
            var newWaterCount = _subject.Pools.Sum(s => s.WaterLevel);

            _subject.CleanPools();
            _subject.SavePools();

            Assert.IsTrue(result, nameof(result));
            Assert.IsTrue(newWaterCount == waterCount + 1, nameof(newWaterCount));
        }

        [Test]
        public async Task ShouldStopFillingPoolsWhenAtMaxCapacity()
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);

            int waterCount = _subject.Pools.Sum(s => s.WaterLevel);

            Assert.IsTrue(waterCount == 0, "Expecting empty pools");

            var maxCapacityAllPools = _subject.Pools.Sum(s => s.MaxWaterLevel);
            for (int i = 0; i < maxCapacityAllPools; i++)
            {
                var result = await _subject.SpawnJobForMostShallowPool(1);
                Assert.IsTrue(result, $"{nameof(i)}: {i}");
            }

            var finalResult = await _subject.SpawnJobForMostShallowPool(1);
            Assert.IsFalse(finalResult, nameof(finalResult));

            var newWaterCount = _subject.Pools.Sum(s => s.WaterLevel);

            _subject.CleanPools();
            _subject.SavePools();

            Assert.IsTrue(newWaterCount == maxCapacityAllPools, nameof(maxCapacityAllPools));
        }

        [Test]
        public async Task ShouldAlternateFillingPools()
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();

            // Should be a total of 2 pools at 0 water level
            Assert.IsTrue(_subject.Pools.Count(c => c.WaterLevel == 0) == 2, "Expecting empty pools");

            await _subject.SpawnJobForMostShallowPool(1);

            // Should be 1 pool with 1 water level, and 1 pool with 0 water level
            Assert.IsTrue(_subject.Pools.Count(c => c.WaterLevel == 1) == 1, "Single spawn, check 1 filled pool");
            Assert.IsTrue(_subject.Pools.Count(c => c.WaterLevel == 0) == 1, "Single spawn, check 1 empty pool");

            await _subject.SpawnJobForMostShallowPool(1);

            // Should be 2 pool with 1 water level (assurring pools are being filled shallow first)
            Assert.IsTrue(_subject.Pools.Count(c => c.WaterLevel == 1) == 2, "Double spawn, check 2 filled pool with 1 value");

            _subject.CleanPools();
            _subject.SavePools();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public async Task ShouldSpawnMultipleFillJobs(int jobsToSpawn)
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            
            _subject = new PoolManager(_mockOptionsPoolConfig.Object, _mockOracle.Object, fullPath, _testPath);
            await _subject.LoadPools();

            var waterCount = _subject.Pools.Sum(s => s.WaterLevel);

            Assert.IsTrue(waterCount == 0, "Expecting empty pools");

            var result = await _subject.SpawnJobForMostShallowPool(jobsToSpawn);
            var newWaterCount = _subject.Pools.Sum(s => s.WaterLevel);

            _subject.CleanPools();
            _subject.SavePools();

            Assert.IsTrue(result, nameof(result));
            Assert.IsTrue(newWaterCount == waterCount + jobsToSpawn, nameof(newWaterCount));
        }
    }
}
