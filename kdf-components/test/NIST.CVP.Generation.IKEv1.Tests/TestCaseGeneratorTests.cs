using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv1.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv1.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSingleBlockEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetIkeMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedIke()
        {
            var ike = GetIkeMock();
            ike
                .Setup(s => s.GenerateIke(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new IkeResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, ike.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionIke()
        {
            var ike = GetIkeMock();
            ike
                .Setup(s => s.GenerateIke(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, ike.Object);

            var result = subject.Generate(new TestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateIkeOperation()
        {
            var ike = GetIkeMock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, ike.Object);

            var result = subject.Generate(new TestGroup(), true);

            ike.Verify(s => s.GenerateIke(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
                Times.AtLeastOnce,
                "GenerateIke should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var ike = GetIkeMock();
            ike
                .Setup(s => s.GenerateIke(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new IkeResult(fakeKey, fakeKey, fakeKey, fakeKey));

            var subject = new TestCaseGenerator(GetRandomMock().Object, ike.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).SKeyId.ToString(), "SKeyId");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SKeyIdD.ToString(), "SKeyIdD");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SKeyIdA.ToString(), "SKeyIdA");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SKeyIdE.ToString(), "SKeyIdE");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(64));
            return random;
        }

        private Mock<IIkeV1> GetIkeMock()
        {
            return new Mock<IIkeV1>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                AuthenticationMethod = AuthenticationMethods.Dsa,
                HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                GxyLength = 64,
                NInitLength = 64,
                NRespLength = 64,
                PreSharedKeyLength = 64
            };
        }
    }
}
