using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DSA.FFC.Tests
{
    [TestFixture, LongRunningIntegrationTest]
    public class UnverifiableGeneratorGeneratorValidatorTests
    {
        [Test]
        [TestCase("", "")]
        public void ShouldGenerateUnverifiableGeneratorsProperly(string pHex, string qHex)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();

            var subject = new UnverifiableGeneratorGeneratorValidator();

            var result = subject.Generate(p, q);
            Assert.IsTrue(result.Success);

            var verified = subject.Validate(p, q, result.G);
            Assert.IsTrue(verified.Success, verified.ErrorMessage);
        }

        [Test]
        [TestCase("", "", "", false)]
        public void ShouldVerifyUnverifiableGeneratorsProperly(string pHex, string qHex, string gHex, bool success)
        {
            var p = new BitString(pHex).ToPositiveBigInteger();
            var q = new BitString(qHex).ToPositiveBigInteger();
            var g = new BitString(gHex).ToPositiveBigInteger();

            var subject = new UnverifiableGeneratorGeneratorValidator();

            var result = subject.Validate(p, q, g);

            Assert.AreEqual(success, result.Success, result.ErrorMessage);
        }
    }
}
