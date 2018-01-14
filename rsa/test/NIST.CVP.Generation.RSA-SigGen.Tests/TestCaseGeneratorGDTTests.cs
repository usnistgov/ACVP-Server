using Moq;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.SHA2;

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

            var primeGen = GetPrimeGenMock();
            primeGen
                .Setup(s => s.GeneratePrimes(It.IsAny<int>(), It.IsAny<BigInteger>(), It.IsAny<BitString>()))
                .Returns(new PrimeGeneratorResult(3, 5));

            var subject = new TestCaseGeneratorGDT(rand.Object, signer.Object, primeGen.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);

            signer.Verify(v => v.Sign(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<KeyPair>()), Times.Once, "Call Sign once");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<SignerBase> GetSignerMock()
        {
            return new Mock<SignerBase>();
        }

        private Mock<PrimeGeneratorBase> GetPrimeGenMock()
        {
            return new Mock<PrimeGeneratorBase>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                Mode = SigGenModes.PSS,
                HashAlg = new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224},
                SaltLen = 40
            };
        }
    }
}
