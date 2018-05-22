using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SRTP.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetSrtpMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedSrtp()
        {
            var srtp = GetSrtpMock();
            srtp
                .Setup(s => s.DeriveKey(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new KdfResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, srtp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionSrtp()
        {
            var srtp = GetSrtpMock();
            srtp
                .Setup(s => s.DeriveKey(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, srtp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateSrtpOperation()
        {
            var srtp = GetSrtpMock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, srtp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            srtp.Verify(s => s.DeriveKey(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
                Times.AtLeastOnce,
                "DeriveKey should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var fakeSrtpResult = new SrtpResult(fakeKey, fakeKey, fakeKey);

            var srtp = GetSrtpMock();
            srtp
                .Setup(s => s.DeriveKey(It.IsAny<int>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new KdfResult(fakeSrtpResult, fakeSrtpResult));

            var subject = new TestCaseGenerator(GetRandomMock().Object, srtp.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtpKe.ToString(), "SrtpKe");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtpKa.ToString(), "SrtpKa");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtpKs.ToString(), "SrtpKs");

            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtcpKe.ToString(), "SrtcpKe");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtcpKa.ToString(), "SrtcpKa");
            Assert.IsNotEmpty(((TestCase)result.TestCase).SrtcpKs.ToString(), "SrtcpKs");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            return random;
        }

        private Mock<ISrtp> GetSrtpMock()
        {
            return new Mock<ISrtp>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                Kdr = new BitString("ABCD"),
                AesKeyLength = 128,
            };
        }
    }
}
