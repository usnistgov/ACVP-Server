using System;
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
        [TestCase(AlgoMode.TDES_CFB1, "TDES-CFB1")]
        [TestCase(AlgoMode.TDES_CFB8, "TDES-CFB8")]
        [TestCase(AlgoMode.TDES_CFB64, "TDES-CFB64")]
        public void CanCreateEnumFromDescription<T>(T value, string description) where T : struct
        {
            Assert.AreEqual(value, EnumHelpers.GetEnumFromEnumDescription<T>(description));
        }

        [TestCase(AlgoMode.TDES_CFB1, "BadEnumDescription")]
        public void ThrowsExceptionForIncorrectDescription<T>(T value, string badDescription) where T : struct
        {
            Assert.Throws(typeof(InvalidOperationException), () => EnumHelpers.GetEnumFromEnumDescription<T>(badDescription));
        }
    }
}
