using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SNMP.Tests
{
    [TestFixture, FastCryptoTest]
    public class SnmpTests
    {
        [Test]
        [TestCase("rWXOgGzq", "000002b87766554433221100", "e1c900949caac3c2029279f21e8294ec0df4a757")]
        [TestCase("SNfkfdNA", "000002b87766554433221100", "29d580910b9394ec7438d507dc768142fc756d10")]
        [TestCase("LPzyFblH", "000002b87766554433221100", "28da1f86ff99490e9cb1040b0e6445cd3ddf9fe6")]
        [TestCase("rQrFXAOAtRLoaaoW", "800002b805123456789abcdef0123456789abcdef0123456789abcdef0123456", "e590a974337f88f110822ded986aad4bd7286697")]
        [TestCase("GSUhAAxiyQobFxYO", "800002b805123456789abcdef0123456789abcdef0123456789abcdef0123456", "1f1bb916ceec8e62fc7b689dc8490665605f42a9")]
        public void ShouldSnmpCorrectly(string password, string engineIdHex, string sharedKeyHex)
        {
            var engineId = new BitString(engineIdHex);
            var sharedKey = new BitString(sharedKeyHex);

            var subject = new Snmp();
            var result = subject.KeyLocalizationFunction(engineId, password);

            Assert.IsTrue(result.Success, result.ErrorMessage);
            Assert.AreEqual(sharedKey.ToHex(), result.SharedKey.ToHex());
        }
    }
}
