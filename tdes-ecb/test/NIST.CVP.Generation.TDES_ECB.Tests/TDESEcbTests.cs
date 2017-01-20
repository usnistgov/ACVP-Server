using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_ECB.Tests
{
    [TestFixture]
    public class TDESEcbTests
    {

        [Test]
        public void ShouldPassStartupTest()
        {
            var keyBytes = new byte[] { 0x01 , 0x23, 0x45, 0x67, 0x89, 0xab , 0xcd, 0xef};
            var plainTextBytes = new byte[] { 0x4e, 0x6f, 0x77, 0x20, 0x69, 0x73, 0x20, 0x74 };
            var expectedBytes = new byte[] { 0x3f, 0xa4, 0x0e, 0x8a, 0x98, 0x4d, 0x48, 0x15 };
            var subject = new TdesEcb();
            var result = subject.BlockEncrypt(new BitString(keyBytes), new BitString(plainTextBytes));
            Assert.IsTrue(result.Success);
            var actual = result.CipherText.ToBytes();
            Assert.AreEqual(expectedBytes, actual);
        }

        [Test]
        [TestCase("0101010101010101", "95f8a5e5dd31d900", "8000000000000000")]
        public void ShouldPassKnownAnswerTest(string key, string plaintext, string cipherText)
        {
            var subject = new TdesEcb();
            var result = subject.BlockEncrypt(new BitString(key), new BitString(plaintext));
            Assert.IsTrue(result.Success);

            Assert.AreEqual(new BitString(cipherText), result.CipherText );
        }
    }
}
