using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.IO;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolTests
    {
        private Mock<IOptions<PoolConfig>> _poolConfig;
        private Mock<IOracle> _oracle;
        private Mock<IPoolRepositoryFactory> _poolRepositoryFactory;
        private Mock<IPoolRepository<AesResult>> _poolRepository;
        private Mock<IPoolLogRepository> _poolLogRepository;
        private Mock<IPoolObjectFactory> _poolObjectFactory;
        private string _testPath;
        private string _fullPath;

        [SetUp]
        public void SetUp()
        {
            _poolConfig = new Mock<IOptions<PoolConfig>>();
            _oracle = new Mock<IOracle>();
            _poolRepositoryFactory = new Mock<IPoolRepositoryFactory>();
            _poolRepository = new Mock<IPoolRepository<AesResult>>();
            _poolLogRepository = new Mock<IPoolLogRepository>();
            _poolObjectFactory = new Mock<IPoolObjectFactory>();

            var poolConfig = new PoolConfig()
            {
                ShouldRecyclePoolWater = false
            };
            _poolConfig.Setup(s => s.Value).Returns(poolConfig);

            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\poolTests\");
            Directory.CreateDirectory(_testPath);
            _fullPath = Path.Combine(_testPath, $"{Guid.NewGuid()}.json");

            _poolRepositoryFactory.Setup(s => s.GetRepository<AesResult>()).Returns(_poolRepository.Object);
            _poolRepository.Setup(s => s.GetPoolCount(It.IsAny<string>(), It.IsAny<bool>())).Returns(0);
            _poolRepository
                .Setup(s => s.GetResultFromPool(It.IsAny<string>()))
                .Returns(() => new PoolObject<AesResult>());
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void ShouldLoadPoolFromJson()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            
            // Add two items to the pool
            pool.AddWater(new AesResult()
            {

            });
            pool.AddWater(new AesResult()
            {

            });

            Assert.AreEqual(2, pool.WaterLevel);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldThrowWhenDirtyWaterAdded(bool cleanWater)
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));

            if (cleanWater)
            {
                Assert.IsTrue(pool.AddWater(new AesResult()));
            }
            else
            {
                Assert.Throws(typeof(ArgumentException), () => pool.AddWater(new HashResult()));
            }
        }

        [Test]
        public void ShouldReturnResultsUponRequest()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var myTestBitString = new BitString("ABCD");
            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            pool.AddWater(new AesResult()
            {
                PlainText = myTestBitString
            });

            _poolRepository
                .Setup(s => s.GetResultFromPool(It.IsAny<string>()))
                .Returns(() => new PoolObject<AesResult>()
                {
                    Value = new AesResult()
                    {
                        PlainText = myTestBitString
                    }
                });

            var result = pool.GetNext();
            
            Assert.IsFalse(result.PoolTooEmpty);
            Assert.AreEqual("ABCD", result.Result.PlainText.ToHex());
        }

        [Test]
        public void ShouldReturnSpecialResultWhenPoolEmpty()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            pool.AddWater(new AesResult()
            {

            });
            pool.AddWater(new AesResult()
            {

            });

            var result1 = pool.GetNext();
            var result2 = pool.GetNext();
            var result3 = pool.GetNext();

            Assert.IsFalse(result1.PoolTooEmpty);
            Assert.IsFalse(result2.PoolTooEmpty);
            Assert.IsTrue(pool.IsEmpty);
            Assert.IsTrue(result3.PoolTooEmpty);
        }
        
        [Test]
        [TestCase(true, 2)]
        [TestCase(false, 1)]
        public void ShouldRecycleValueWhenSpecified(bool shouldRecycle, int waterLevelPostValueGet)
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            _poolConfig.Setup(s => s.Value)
                .Returns(new PoolConfig() { ShouldRecyclePoolWater = shouldRecycle });

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            
            pool.AddWater(new AesResult() { PlainText = new BitString("01") });
            pool.AddWater(new AesResult() { PlainText = new BitString("02") });
            var addedWater = 2; // as per above water adds

            pool.GetNextUntyped();

            _poolRepository
                .Verify(
                    v => v.AddResultToPool(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<PoolObject<AesResult>>()),
                    Times.Exactly(shouldRecycle ? addedWater + 1 : addedWater + 0),
                    nameof(_poolRepository.Object.AddResultToPool)
                );
        }

        [Test]
        public void ShouldCleanPool()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            pool.AddWater(new AesResult
            {

            });
            
            Assume.That(!pool.IsEmpty);

            pool.CleanPool();

            Assert.IsTrue(pool.IsEmpty);
        }

        private PoolConstructionParameters<TParam> GetConstructionParameters<TParam>(TParam param, PoolTypes poolType)
            where TParam : IParameters
        {
            return new PoolConstructionParameters<TParam>
            {
                PoolConfig = _poolConfig.Object,
                PoolProperties = new PoolProperties
                {
                    PoolName = _fullPath,
                    MaxCapacity = 100,
                    MinCapacity = 1,
                    RecycleRatePerHundred = 100,
                    PoolType = new ParameterHolder
                    {
                        Parameters = param,
                        Type = poolType
                    }
                },
                WaterType = param,
                PoolName = _fullPath,
                Oracle = _oracle.Object,
                PoolLogRepository = _poolLogRepository.Object,
                PoolObjectFactory = _poolObjectFactory.Object,
                PoolRepositoryFactory = _poolRepositoryFactory.Object
            };
        }
    }
}
