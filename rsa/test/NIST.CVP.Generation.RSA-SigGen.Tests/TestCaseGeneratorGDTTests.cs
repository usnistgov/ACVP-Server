using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorGDTTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorGDT(GetRandomMock().Object, GetSignatureBuilderMock().Object, GetKeyBuilderMock().Object, GetPaddingFactoryMock().Object, GetShaFactoryMock().Object, GetKeyComposerFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignatureIfIsSample()
        {
            var signer = GetSignatureBuilderMock();
            signer
                .Setup(s => s.BuildSign())
                .Returns(new SignatureResult(1));

            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("BEEFFACE"));

            var keyBuilder = GetKeyBuilderMock();
            keyBuilder
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair()));

            var subject = new TestCaseGeneratorGDT(rand.Object, signer.Object, keyBuilder.Object, GetPaddingFactoryMock().Object, GetShaFactoryMock().Object, GetKeyComposerFactoryMock().Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success);

            signer.Verify(v => v.BuildSign(), Times.Once, "Call Sign once");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<ISignatureBuilder> GetSignatureBuilderMock()
        {
            return new Mock<ISignatureBuilder>();
        }

        private Mock<IKeyBuilder> GetKeyBuilderMock()
        {
            return new Mock<IKeyBuilder>();
        }

        private Mock<IPaddingFactory> GetPaddingFactoryMock()
        {
            return new Mock<IPaddingFactory>();
        }

        private Mock<IShaFactory> GetShaFactoryMock()
        {
            return new Mock<IShaFactory>();
        }

        private Mock<IKeyComposerFactory> GetKeyComposerFactoryMock()
        {
            return new Mock<IKeyComposerFactory>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Modulo = 2048,
                Mode = SignatureSchemes.Pss,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                SaltLen = 40
            };
        }
    }
}
