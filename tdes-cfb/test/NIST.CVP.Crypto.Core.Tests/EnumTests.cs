using NIST.CVP.Tests.Core.TestCategoryAttributes;
using System;
using System.ComponentModel;
using NUnit.Framework;
using NIST.CVP.Crypto.TDES_CFB;

namespace NIST.CVP.Crypto.Core.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class EnumExTests
    {
        [Test]
        [TestCase(Algo.TDES_CFB1, "TDES-CFB1")]
        [TestCase(Algo.TDES_CFB8, "TDES-CFB8")]
        [TestCase(Algo.TDES_CFB64, "TDES-CFB64")]
        public void CanCreateEnumFromDescription<T>(T _enum, string Description)
        {
            Assert.AreEqual(_enum, EnumEx.FromDescription<T>(Description));
        }

        [TestCase(Algo.TDES_CFB1, "BadEnumDescription")]
        public void ThrowsExceptionForIncorrectDescription<T>(T _enum, string BadDescription)
        {
            Assert.Throws<InvalidEnumArgumentException>(() => EnumEx.FromDescription<T>(BadDescription));
        }
    }
}
