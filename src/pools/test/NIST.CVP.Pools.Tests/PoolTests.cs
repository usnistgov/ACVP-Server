using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Pools.PoolModels;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using Moq;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolTests
    {

        private readonly Mock<IOptions<PoolConfig>> _poolConfig = new Mock<IOptions<PoolConfig>>();
        private string _testPath;
        private string _configFile = "aes-test1.json";
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

            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\");
            _fullPath = Path.Combine(_testPath, _configFile);
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

            var result = pool.GetNext();
            
            Assert.IsFalse(result.PoolEmpty);
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

            var result1 = pool.GetNext();
            var result2 = pool.GetNext();
            var result3 = pool.GetNext();

            Assert.IsFalse(result1.PoolEmpty);
            Assert.IsFalse(result2.PoolEmpty);
            Assert.IsTrue(pool.IsEmpty);
            Assert.IsTrue(result3.PoolEmpty);
        }

        [Test]
        public void ShouldWriteToFile()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));

            var writePath = Path.Combine(_testPath, $"saveTest-{Guid.NewGuid().ToString().Substring(0, 8)}.json");
            pool.SavePoolToFile(writePath);

            if (File.Exists(writePath))
            {
                File.Delete(writePath);
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
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
                .Returns(new PoolConfig() {ShouldRecyclePoolWater = shouldRecycle});

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));
            var originalWaterLevel = pool.WaterLevel;
            
            pool.AddWater(new AesResult() {PlainText = new BitString("01")});
            pool.AddWater(new AesResult() {PlainText = new BitString("02")});

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
            Assume.That(!pool.IsEmpty);

            pool.CleanPool();

            Assert.IsTrue(pool.IsEmpty);
        }

        [Test]
        public void ShouldIncrementWaterUsageCount()
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            _poolConfig.Setup(s => s.Value).Returns(new PoolConfig() {ShouldRecyclePoolWater = true});

            var pool = new AesPool(GetConstructionParameters(param, PoolTypes.AES));

            pool.CleanPool();
            Assume.That(pool.IsEmpty);

            pool.AddWater(new AesResult() {PlainText = new BitString("01")});

            var result = pool.GetNextUntyped();
            Assert.AreEqual(0, result.TimesValueUsed);
            for (int i = 0; i < 5; i++)
            {
                result = pool.GetNextUntyped();
                Assert.AreEqual(i + 1, result.TimesValueUsed);
            }
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldReuseValueUntilReachingMaxReuseValue(int maxReuse)
        {
            var param = new AesParameters
            {
                Direction = "encrypt",
                DataLength = 128,
                Mode = BlockCipherModesOfOperation.Ecb,
                KeyLength = 128
            };

            _poolConfig.Setup(s => s.Value).Returns(new PoolConfig() {ShouldRecyclePoolWater = true});

            var constructionParameters = GetConstructionParameters(param, PoolTypes.AES);
            constructionParameters.PoolProperties.MaxWaterReuse = maxReuse;
            var pool = new AesPool(constructionParameters);

            pool.CleanPool();
            Assume.That(pool.IsEmpty);

            pool.AddWater(new AesResult() {PlainText = new BitString("01")});

            Assume.That(!pool.IsEmpty);

            pool.GetNextUntyped();
            for (int i = 0; i < maxReuse; i++)
            {
                Assert.IsFalse(pool.IsEmpty);
                pool.GetNextUntyped();
            }

            Assert.IsTrue(pool.IsEmpty);
        }

        private PoolConstructionParameters<TParam> GetConstructionParameters<TParam>(TParam param, PoolTypes poolType)
            where TParam : IParameters
        {
            return new PoolConstructionParameters<TParam>()
            {
                JsonConverters = _jsonConverters,
                PoolConfig = _poolConfig.Object,
                PoolProperties = new PoolProperties()
                {
                    FilePath = _fullPath,
                    MaxCapacity = 100,
                    MaxWaterReuse = 1000,
                    MonitorFrequency = 30,
                    PoolType = new ParameterHolder()
                    {
                        Parameters = param,
                        Type = poolType
                    }
                },
                WaterType = param,
                FullPoolLocation = _fullPath
            };
        }
    }
}
