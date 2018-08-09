using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core.JsonConverters;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolTests
    {
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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath, _jsonConverters);

            Assert.AreEqual(2, pool.WaterLevel);
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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath, _jsonConverters);

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath, _jsonConverters);

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath, _jsonConverters);

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
    }
}
