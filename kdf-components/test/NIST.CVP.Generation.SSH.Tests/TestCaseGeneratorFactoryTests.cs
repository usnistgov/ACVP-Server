using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SSH.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SSH.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        public void ShouldReturnProperGenerator()
        {
            var testGroup = new TestGroup
            {
                Cipher = Cipher.AES128,
                HashAlg =  new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TestType = "aft"
            };

            var subject = new TestCaseGeneratorFactory(null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(typeof(TestCaseGenerator), generator);
        }
    }
}
