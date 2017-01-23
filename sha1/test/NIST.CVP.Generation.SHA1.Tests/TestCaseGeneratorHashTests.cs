using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Moq;
using NUnit.Framework;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorHashTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorHash(GetRandomMock().Object, GetSHAMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var sha = GetSHAMock();
            sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            TestCaseGeneratorHash subject =
                new TestCaseGeneratorHash(GetRandomMock().Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var sha = GetSHAMock();
            sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Throws(new Exception());

            TestCaseGeneratorHash subject =
                new TestCaseGeneratorHash(GetRandomMock().Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var sha = GetSHAMock();

            TestCaseGeneratorHash subject =
                new TestCaseGeneratorHash(GetRandomMock().Object, sha.Object);

            var result = subject.Generate(new TestGroup(), true);

            sha.Verify(v => v.HashMessage(It.IsAny<BitString>()),
                Times.AtLeastOnce,
                "HashMessage should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeHash = new BitString(new byte[] { 1 });
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 2 }));

            var sha = GetSHAMock();
            sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult(fakeHash));

            TestCaseGeneratorHash subject =
                new TestCaseGeneratorHash(random.Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Message.ToString(), "Message");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Digest.ToString(), "Digest");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<ISHA1> GetSHAMock()
        {
            return new Mock<ISHA1>();
        }
    }
}
