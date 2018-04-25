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
        [TestCase("aEs", "cBc", AlgoMode.AES_CBC)]
        [TestCase("aes", "gcm", AlgoMode.AES_GCM)]
        [TestCase("ctrDRBG", "", AlgoMode.DRBG_CTR)]
        [TestCase("hashDRBG", "", AlgoMode.DRBG_Hash)]
        [TestCase("KDF-Components", "ANSIX9.63", AlgoMode.KDFComponents_ANSIX963)]
        public void ShouldGetCorrectAlgoModeFromStrings(string algo, string mode, AlgoMode expected)
        {
            var result = AlgoModeLookupHelper.GetAlgoModeFromStrings(algo, mode);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldThrowWhenAlgoNull()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(null, ""));
        }

        [Test]
        public void ShouldThrowWhenAlgoEmpty()
        {
            Assert.Throws(typeof(ArgumentNullException), 
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings(string.Empty, ""));
        }

        [Test]
        public void ShouldThrowWhenAlgoModeCombinationInvalid()
        {
            Assert.Throws(typeof(InvalidOperationException),
                () => AlgoModeLookupHelper.GetAlgoModeFromStrings("AES", "Invalid"));
        }
    }
}
