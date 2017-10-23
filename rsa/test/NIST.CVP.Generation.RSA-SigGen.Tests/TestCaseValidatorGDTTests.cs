using NIST.CVP.Tests.Core.TestCategoryAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NIST.CVP.Crypto.RSA.Signatures;
using Moq;
using NIST.CVP.Math;
using NIST.CVP.Crypto.RSA;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorGDTTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodSignature()
        {
            var mockSigner = new Mock<SignerBase>();
            mockSigner
                .Setup(s => s.Verify(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>(), It.IsAny<BitString>()))
                .Returns(new VerifyResult());

            var subject = new TestCaseValidatorGDT(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = subject.Validate(GetResultTestCase());

            mockSigner.Verify(v => v.Verify(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>(), It.IsAny<BitString>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadSignature()
        {
            var mockSigner = new Mock<SignerBase>();
            mockSigner
                .Setup(s => s.Verify(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>(), It.IsAny<BitString>()))
                .Returns(new VerifyResult("Fail"));

            var subject = new TestCaseValidatorGDT(GetTestCase(), GetTestGroup(), mockSigner.Object);
            var result = subject.Validate(GetResultTestCase());

            mockSigner.Verify(v => v.Verify(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>(), It.IsAny<BitString>()), Times.Once);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                Message = new BitString("ABCD"),
                TestCaseId = 1
            };
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Mode = SigGenModes.ANS_931,
                Modulo = 2048,
                Key = new KeyPair(5, 7, 3)
            };
        }

        private TestCase GetResultTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Signature = new BitString("ABCD")
            };
        }
    }
}
