using System;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Exceptions;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class AlgoModeLookupHelperTests
    {
        [Test]
        [TestCase("acvp-aEs", "cBc", "1.0", AlgoMode.AES_CBC_v1_0)]
        [TestCase("acvp-aes", "gcm", "1.0", AlgoMode.AES_GCM_v1_0)]
        [TestCase("ctrDRBG", null, "1.0", AlgoMode.DRBG_CTR_v1_0)]
        [TestCase("hashDRBG", null, "1.0", AlgoMode.DRBG_Hash_v1_0)]
        [TestCase("KDF-Components", "ANSIX9.63", "1.0", AlgoMode.KDFComponents_ANSIX963_v1_0)]
        public void ShouldGetCorrectAlgoModeFromStrings(string algo, string mode, string revision, AlgoMode expected)
        {
            var result = AlgoModeLookupHelper.GetAlgoModeFromStrings(algo, mode, revision);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void ShouldThrowWhenAlgoNull()
        {
            Assert.Throws(typeof(AlgoModeRevisionException),
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(null, null, "1.0"));
        }

        [Test]
        public void ShouldThrowWhenAlgoEmpty()
        {
            Assert.Throws(typeof(AlgoModeRevisionException),
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(string.Empty, null, "1.0"));
        }

        [Test]
        public void ShouldThrowWhenAlgoModeCombinationInvalid()
        {
            Assert.Throws(typeof(AlgoModeRevisionException),
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings("AES", "Invalid", "1.0"));
        }
    }
}
