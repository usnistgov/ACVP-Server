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
using NIST.CVP.Pools.Interfaces;
using NIST.CVP.Pools.Models;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Pools.PoolModels;
using NIST.CVP.Pools.Services;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolManagerTests
    {
        private Mock<IOptions<PoolConfig>> _mockOptionsPoolConfig;
        private Mock<IPoolRepositoryFactory> _mockPoolRepositoryFactory;
        private Mock<IPoolLogRepository> _mockPoolLogRepository;
        private Mock<IPoolRepository<AesResult>> _mockPoolAesRepository;
        private Mock<IPoolRepository<HashResult>> _mockPoolShaRepository;
        private Mock<IPoolFactory> _mockPoolFactory;
        private IPoolFactory _poolFactory;
        private Mock<IPool> _mockPool;
        private Mock<IJsonConverterProvider> _mockJsonConverterProvider = new Mock<IJsonConverterProvider>();
        private readonly PoolConfig _poolConfig = new PoolConfig()
        {
            Port = 42,
            RootUrl = "localhost",
            ShouldRecyclePoolWater = false,
        };
        private string _testPath;
        private string _configFile = "testConfig.json";
        private PoolManager _subject;

        [SetUp]
        public void SetUp()
        {
            _mockOptionsPoolConfig = new Mock<IOptions<PoolConfig>>();
            _mockPoolRepositoryFactory = new Mock<IPoolRepositoryFactory>();
            _mockPoolLogRepository = new Mock<IPoolLogRepository>();
            _mockPoolAesRepository = new Mock<IPoolRepository<AesResult>>();
            _mockPoolShaRepository = new Mock<IPoolRepository<HashResult>>();
            _mockPoolFactory = new Mock<IPoolFactory>();
            _poolFactory = new PoolFactory(
                _mockOptionsPoolConfig.Object, 
                new Mock<IOracle>().Object,
                _mockPoolRepositoryFactory.Object, 
                _mockPoolLogRepository.Object,
                new Mock<IPoolObjectFactory>().Object
            );
            _mockPool = new Mock<IPool>();
            _mockJsonConverterProvider = new Mock<IJsonConverterProvider>();
            
            _mockOptionsPoolConfig.Setup(s => s.Value).Returns(_poolConfig);
            _mockPoolRepositoryFactory
                .Setup(s => s.GetRepository<AesResult>())
                .Returns(() => _mockPoolAesRepository.Object);
            _mockPoolRepositoryFactory
                .Setup(s => s.GetRepository<HashResult>())
                .Returns(() => _mockPoolShaRepository.Object);
            _mockPoolAesRepository.Setup(s => s.GetPoolCount(It.IsAny<string>(), It.IsAny<bool>())).Returns(0);
            _mockPoolShaRepository.Setup(s => s.GetPoolCount(It.IsAny<string>(), It.IsAny<bool>())).Returns(0);
            _mockPoolFactory.Setup(s => s.GetPool(It.IsAny<PoolProperties>())).Returns(_mockPool.Object);
            _mockPool.Setup(s => s.Param).Returns(() => new AesParameters());
            _mockPool.Setup(s => s.WaterLevel).Returns(0);

            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
            _poolConfig.PoolConfigFile = Path.Combine(_testPath, _configFile);
            _poolConfig.PoolDirectory = _testPath;
            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _poolFactory, 
                _mockJsonConverterProvider.Object
            );
        }

        [Test]
        public void ShouldLoadConfig()
        {
            Assert.AreEqual(
                3, 
                _subject.Pools.Count(),
                "pool count");
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

            _subject.AddResultToPool(paramHolder);

            _mockPoolAesRepository.Verify(
                v => v.AddResultToPool(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<PoolObject<AesResult>>()),
                Times.Once,
                nameof(_mockPoolAesRepository.Object.AddResultToPool)
            );
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
            var aesResult = new AesResult()
            {
                CipherText = new BitString("01"),
                Iv = new BitString("02"),
                Key = new BitString("03"),
                PlainText = new BitString("04")
            };

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
                Result = aesResult
            };

            _mockPoolAesRepository
                .Setup(s => s.GetResultFromPool(It.IsAny<string>()))
                .Returns(() => new PoolObject<AesResult>()
                {
                    Value = aesResult
                });

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
                PoolName = prechangeConfig.PoolName,
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
        public void ShouldProperlySaveConfigurationFile()
        {
            var fullPath = Path.Combine(_testPath, "saveChangesConfig.json");
            _poolConfig.PoolConfigFile = fullPath;

            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _mockPoolFactory.Object, 
                _mockJsonConverterProvider.Object
            );
            
            // Change the pool configuration
            var prechangeConfig = _subject.GetPoolProperties().First();
            // copy of object as to not work with the original reference
            var prechangeConfigCopy = new PoolProperties()
            {
                PoolName = prechangeConfig.PoolName,
                MaxCapacity = prechangeConfig.MaxCapacity,
                MinCapacity = prechangeConfig.MinCapacity,
                RecycleRatePerHundred = prechangeConfig.RecycleRatePerHundred,
                PoolType = prechangeConfig.PoolType
            };
            var newConfig = new PoolProperties()
            {
                PoolName = prechangeConfigCopy.PoolName,
                MaxCapacity = prechangeConfigCopy.MaxCapacity,
                MinCapacity = prechangeConfigCopy.MinCapacity + 42,
                RecycleRatePerHundred = prechangeConfigCopy.RecycleRatePerHundred,
                PoolType = prechangeConfigCopy.PoolType
            };
            _subject.EditPoolProperties(newConfig);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _mockPoolFactory.Object, 
                _mockJsonConverterProvider.Object
            );
            
            var postChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(newConfig.MinCapacity, postChangeConfig.MinCapacity, "config change");

            // Change the value back
            _subject.EditPoolProperties(prechangeConfigCopy);
            _subject.SavePoolConfigs();

            // Reinitialize pools
            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _mockPoolFactory.Object, 
                _mockJsonConverterProvider.Object
            );
            
            var validateOriginalChangeConfig = _subject.GetPoolProperties().First();

            Assert.AreEqual(prechangeConfigCopy.MinCapacity, validateOriginalChangeConfig.MinCapacity, "original file check");
        }

        [Test]
        public async Task ShouldFillPoolWhenNotAtCapacity()
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            _poolConfig.PoolConfigFile = fullPath;

            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _poolFactory, 
                _mockJsonConverterProvider.Object
            );
            
            var waterCount = _subject.Pools.Sum(s => s.WaterLevel);

            Assert.IsTrue(waterCount == 0, "Expecting empty pools");

            var result = await _subject.SpawnJobForMostShallowPool(1);
            var newWaterCount = _subject.Pools.Sum(s => s.WaterLevel);

            _subject.CleanPools();

            Assert.IsTrue(result.HasSpawnedJob, nameof(result));
            Assert.IsTrue(newWaterCount == waterCount + 1, nameof(newWaterCount));
        }

        [Test]
        public async Task ShouldStopFillingPoolsWhenAtMaxCapacity()
        {
            var fullPath = Path.Combine(_testPath, "fillPoolConfig.json");
            _poolConfig.PoolConfigFile = fullPath;

            _subject = new PoolManager(
                _mockOptionsPoolConfig.Object, 
                _mockPoolLogRepository.Object, 
                _poolFactory, 
                _mockJsonConverterProvider.Object
            );

            var waterCount = _subject.Pools.Sum(s => s.WaterLevel);

            Assert.IsTrue(waterCount == 0, "Expecting empty pools");

            var maxCapacityAllPools = _subject.Pools.Sum(s => s.MaxWaterLevel);
            for (int i = 0; i < maxCapacityAllPools; i++)
            {
                var result = await _subject.SpawnJobForMostShallowPool(1);
                Assert.IsTrue(result.HasSpawnedJob, $"{nameof(i)}: {i}");
            }

            var finalResult = await _subject.SpawnJobForMostShallowPool(1);
            Assert.IsFalse(finalResult.HasSpawnedJob, nameof(finalResult));

            var newWaterCount = _subject.Pools.Sum(s => s.WaterLevel);

            _subject.CleanPools();

            Assert.IsTrue(newWaterCount == maxCapacityAllPools, nameof(maxCapacityAllPools));
        }
    }
}
