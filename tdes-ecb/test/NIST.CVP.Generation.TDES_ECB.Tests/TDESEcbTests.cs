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
