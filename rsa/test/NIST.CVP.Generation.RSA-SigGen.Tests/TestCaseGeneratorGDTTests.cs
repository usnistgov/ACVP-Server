using Moq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGDTTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorGDT(GetRandomMock().Object, GetSignerMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignatureIfIsSample()
        {
            var signer = GetSignerMock();
            signer
                .Setup(s => s.Sign(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>()))
                .Returns(new SignatureResult(new BitString("ABCD")));

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var subject = new TestCaseGeneratorGDT(rand.Object, signer.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);

            signer.Verify(v => v.Sign(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>()), Times.Once, "Call Sign once");
        }

        [Test]
        public void GeneratedTestCaseShouldContainSaltValue()
        {
            var subject = new TestCaseGeneratorGDT(GetRandomMock().Object, GetSignerMock().Object);
            var group = GetTestGroup();
            group.SaltMode = SaltModes.FIXED;
            group.Salt = new BitString("ABCD");

            var result = subject.Generate(group, false);

            Assume.That(result.Success);

            var testCase = (TestCase)result.TestCase;

            Assert.IsNotNull(testCase, $"{nameof(testCase)} should not be null");
            Assert.AreEqual(new BitString("ABCD"), testCase.Salt);
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<SignerBase> GetSignerMock()
        {
            return new Mock<SignerBase>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                Mode = SigGenModes.PSS,
                HashAlg = new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224},
                SaltMode = SaltModes.RANDOM,
                SaltLen = 40
            };
        }
    }
}
