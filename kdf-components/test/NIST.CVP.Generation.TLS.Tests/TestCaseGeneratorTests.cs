using System;
using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TLS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetTlsMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedTls()
        {
            var tls = GetTlsMock();
            tls
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new TlsKdfResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, tls.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionTls()
        {
            var tls = GetTlsMock();
            tls
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, tls.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateTlsOperation()
        {
            var tls = GetTlsMock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, tls.Object);

            var result = subject.Generate(new TestGroup(), true);

            tls.Verify(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "DeriveKey should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var tls = GetTlsMock();
            tls
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new TlsKdfResult(fakeKey, fakeKey));

            var subject = new TestCaseGenerator(GetRandomMock().Object, tls.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).MasterSecret.ToString(), "MasterSecret");
            Assert.IsNotEmpty(((TestCase)result.TestCase).KeyBlock.ToString(), "KeyBlock");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            return random;
        }

        private Mock<ITlsKdf> GetTlsMock()
        {
            return new Mock<ITlsKdf>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                TlsMode = TlsModes.v10v11,
                PreMasterSecretLength = 128,
                KeyBlockLength = 128
            };
        }
    }
}
