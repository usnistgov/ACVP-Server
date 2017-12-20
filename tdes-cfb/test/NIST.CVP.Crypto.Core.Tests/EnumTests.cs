using NIST.CVP.Tests.Core.TestCategoryAttributes;
using System.ComponentModel;
using NIST.CVP.Crypto.Common;
using NUnit.Framework;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Crypto.Core.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class EnumExTests
    {
        [Test]
        [TestCase(Algo.TDES_CFB1, "TDES-CFB1")]
        [TestCase(Algo.TDES_CFB8, "TDES-CFB8")]
        [TestCase(Algo.TDES_CFB64, "TDES-CFB64")]
        public void CanCreateEnumFromDescription<T>(T _enum, string Description) where T : struct
        {
            Assert.AreEqual(_enum, EnumHelpers.GetEnumFromEnumDescription<T>(Description));
        }

        [TestCase(Algo.TDES_CFB1, "BadEnumDescription")]
        public void ThrowsExceptionForIncorrectDescription<T>(T _enum, string BadDescription) where T : struct
        {
            Assert.Throws<InvalidEnumArgumentException>(() => EnumHelpers.GetEnumFromEnumDescription<T>(BadDescription));
        }
    }
}
