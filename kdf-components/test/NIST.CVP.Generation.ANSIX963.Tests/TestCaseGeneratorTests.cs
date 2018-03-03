using System;
using Moq;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetAnsiX963Mock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedAnsiX963()
        {
            var ansx = GetAnsiX963Mock();
            ansx
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new KdfResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, ansx.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionAnsiX963()
        {
            var ansx = GetAnsiX963Mock();
            ansx
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, ansx.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateAnsiX963Operation()
        {
            var ansx = GetAnsiX963Mock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, ansx.Object);

            var result = subject.Generate(GetTestGroup(), true);

            ansx.Verify(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "DeriveKey should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var fakeAnsiX963Result = new KdfResult(fakeKey);

            var ansx = GetAnsiX963Mock();
            ansx
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(fakeAnsiX963Result);

            var subject = new TestCaseGenerator(GetRandomMock().Object, ansx.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).KeyData.ToString(), "KeyData");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            return random;
        }

        private Mock<IAnsiX963> GetAnsiX963Mock()
        {
            return new Mock<IAnsiX963>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                KeyDataLength = 128,
                FieldSize = 256,
                SharedInfoLength = 256
            };
        }
    }
}
