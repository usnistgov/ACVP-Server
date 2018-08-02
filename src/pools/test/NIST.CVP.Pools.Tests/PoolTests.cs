using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Tests.Core;
using NUnit.Framework;
using System;
using System.IO;

namespace NIST.CVP.Pools.Tests
{
    [TestFixture]
    public class PoolTests
    {
        private string _testPath;
        private string _configFile = "aes-test1.json";
        private string _fullPath;

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath);

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath);

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath);

            var result1 = pool.GetNext();
            var result2 = pool.GetNext();
            var result3 = pool.GetNext();

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

            var pool = new Pool<AesParameters, AesResult>(param, _fullPath);

            var writePath = Path.Combine(_testPath, $"saveTest-{Guid.NewGuid().ToString().Substring(0, 8)}.json");
            pool.SavePoolToFile(writePath);

            Assert.IsTrue(File.Exists(writePath));
            //File.Delete(writePath);
        }
    }
}
