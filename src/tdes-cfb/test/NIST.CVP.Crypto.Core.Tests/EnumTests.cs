using System;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Common;
using NUnit.Framework;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Crypto.Core.Tests
{
    [TestFixture, UnitTest, FastIntegrationTest]
    public class EnumExTests
    {
        [Test]
        [TestCase(AlgoMode.TDES_CFB1_v1_0, "TDES-CFB1-1.0")]
        [TestCase(AlgoMode.TDES_CFB8_v1_0, "TDES-CFB8-1.0")]
        [TestCase(AlgoMode.TDES_CFB64_v1_0, "TDES-CFB64-1.0")]
        public void CanCreateEnumFromDescription<T>(T value, string description) where T : struct
        {
            Assert.AreEqual(value, EnumHelpers.GetEnumFromEnumDescription<T>(description));
        }

        [TestCase(AlgoMode.TDES_CFB1_v1_0, "BadEnumDescription")]
        public void ThrowsExceptionForIncorrectDescription<T>(T value, string badDescription) where T : struct
        {
            Assert.Throws(typeof(InvalidOperationException), () => EnumHelpers.GetEnumFromEnumDescription<T>(badDescription));
        }
    }
}
