using System;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class AlgoModeLookupHelperTests
    {
        [Test]
        [TestCase("aEs", "cBc", "1.0", AlgoMode.AES_CBC_v1_0)]
        [TestCase("aes", "gcm", "1.0", AlgoMode.AES_GCM_v1_0)]
        [TestCase("ctrDRBG", "", "1.0", AlgoMode.DRBG_CTR_v1_0)]
        [TestCase("hashDRBG", "", "1.0", AlgoMode.DRBG_Hash_v1_0)]
        [TestCase("KDF-Components", "ANSIX9.63", "1.0", AlgoMode.KDFComponents_ANSIX963_v1_0)]
        public void ShouldGetCorrectAlgoModeFromStrings(string algo, string mode, string revision, AlgoMode expected)
        {
            var result = AlgoModeLookupHelper.GetAlgoModeFromStrings(algo, mode, revision);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldThrowWhenAlgoNull()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(null, "", ""));
        }

        [Test]
        public void ShouldThrowWhenAlgoEmpty()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(string.Empty, "", ""));
        }

        [Test]
        public void ShouldThrowWhenAlgoModeCombinationInvalid()
        {
            Assert.Throws(typeof(InvalidOperationException),
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings("AES", "Invalid", ""));
        }
    }
}
