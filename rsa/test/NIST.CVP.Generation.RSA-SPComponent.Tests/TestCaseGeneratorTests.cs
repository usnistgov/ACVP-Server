using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Crypto.RSA2.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var rand = GetRandomMock();
            rand
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString("ABCDEFABCDEF"));        // Must be between 32-64 bits

            var subject = new TestCaseGenerator(rand.Object, GetKeyBuilderMock().Object, GetRsaMock().Object, GetKeyComposerFactoryMock().Object);
            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldGenerateSignatureIfNotFailureTest()
        {
            var rsa = GetRsaMock();
            rsa
                .Setup(s => s.Decrypt(It.IsAny<BigInteger>(), It.IsAny<PrivateKeyBase>(), It.IsAny<PublicKey>()))
                .Returns(7);

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetKeyBuilderMock().Object, rsa.Object, GetKeyComposerFactoryMock().Object);

            var testCase = new TestCase
            {
                Key = new KeyPair(),
                Message = new BitString("ABCD"),
                FailureTest = false
            };

            var result = subject.Generate(GetTestGroup(), testCase);

            Assert.IsTrue(result.Success);

            rsa.Verify(v => v.Decrypt(It.IsAny<BigInteger>(), It.IsAny<PrivateKeyBase>(), It.IsAny<PublicKey>()), Times.Once, "Call Decrypt once");
        }

        [Test]
        public void GenerateShouldNotGenerateSignatureIfFailureTest()
        {
            var rsa = GetRsaMock();
            rsa
                .Setup(s => s.Decrypt(It.IsAny<BigInteger>(), It.IsAny<PrivateKeyBase>(), It.IsAny<PublicKey>()))
                .Returns(7);

            var subject = new TestCaseGenerator(GetRandomMock().Object, GetKeyBuilderMock().Object, rsa.Object, GetKeyComposerFactoryMock().Object);

            var testCase = new TestCase
            {
                Key = new KeyPair { PrivKey = new CrtPrivateKey(), PubKey = new PublicKey() },
                Message = new BitString("ABCD"),
                FailureTest = true
            };

            var result = subject.Generate(GetTestGroup(), testCase);

            Assert.IsTrue(result.Success);

            rsa.Verify(v => v.Decrypt(It.IsAny<BigInteger>(), It.IsAny<PrivateKeyBase>(), It.IsAny<PublicKey>()), Times.Never, "Call Decrypt never");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IRsa> GetRsaMock()
        {
            return new Mock<IRsa>();
        }

        private Mock<IKeyBuilder> GetKeyBuilderMock()
        {
            var mock = new Mock<IKeyBuilder>();
            mock
                .Setup(s => s.Build())
                .Returns(new KeyResult(new KeyPair { PubKey = new PublicKey{N = 10}}, new AuxiliaryResult()));
            
            mock.SetReturnsDefault(mock.Object);
            
            return mock;
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
                KeyFormat = PrivateKeyModes.Crt
            };
        }
    }
}
