using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using NIST.CVP.Pools.PoolModels;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolTests
    {
        private readonly Mock<IOptions<PoolConfig>> _poolConfig = new Mock<IOptions<PoolConfig>>();
        private string _testPath;
        private string _fullPath;

        private readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>
        {
            new BitstringConverter(),
            new DomainConverter(),
            new BigIntegerConverter()
        };

        [SetUp]
        public void SetUp()
        {
            var poolConfig = new PoolConfig()
            {
                ShouldRecyclePoolWater = false
            };
            _poolConfig.Setup(s => s.Value).Returns(poolConfig);

            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\poolTests\");
            Directory.CreateDirectory(_testPath);
            _fullPath = Path.Combine(_testPath, $"{Guid.NewGuid()}.json");
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

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            pool.AddWater(new AesResult()
            {
                PlainText = new BitString("ABCD")
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
            var originalWaterLevel = pool.WaterLevel;

            pool.AddWater(new AesResult() { PlainText = new BitString("01") });
            pool.AddWater(new AesResult() { PlainText = new BitString("02") });

            Assert.AreEqual(
                originalWaterLevel + 2,
                pool.WaterLevel,
                "Water level sanity check, post add"
            );

            pool.GetNextUntyped();

            Assert.AreEqual(
                originalWaterLevel + waterLevelPostValueGet,
                pool.WaterLevel,
                nameof(waterLevelPostValueGet)
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
                JsonConverters = _jsonConverters,
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
                PoolName = _fullPath
            };
        }
    }
}
