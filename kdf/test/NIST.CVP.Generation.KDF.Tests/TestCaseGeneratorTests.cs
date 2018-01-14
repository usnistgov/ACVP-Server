using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KDF.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSingleBlockEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetKdfMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedKdf()
        {
            var kdf = GetKdfMock();
            kdf
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new KdfResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, kdf.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionKdf()
        {
            var kdf = GetKdfMock();
            kdf
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, kdf.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var kdf = GetKdfMock();
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(0));

            var subject = new TestCaseGenerator(GetRandomMock().Object, kdf.Object);

            var result = subject.Generate(new TestGroup(), true);

            kdf.Verify(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "DeriveKey should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var kdf = GetKdfMock();
            kdf
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new KdfResult(fakeKey));

            var subject = new TestCaseGenerator(GetRandomMock().Object, kdf.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).KeyOut.ToString(), "KeyOut");
            Assert.IsNotEmpty(((TestCase)result.TestCase).KeyIn.ToString(), "KeyIn");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty(((TestCase)result.TestCase).FixedData.ToString(), "FixedData");
            Assert.IsTrue(result.TestCase.Deferred, "Deferred");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            return random;
        }

        private Mock<IKdf> GetKdfMock()
        {
            return new Mock<IKdf>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                KdfMode = KdfModes.Feedback,
                MacMode = MacModes.CMAC_TDES,
                CounterLength = 16,
                CounterLocation = CounterLocations.BeforeIterator,
                KeyOutLength = 128
            };
        }
    }
}
