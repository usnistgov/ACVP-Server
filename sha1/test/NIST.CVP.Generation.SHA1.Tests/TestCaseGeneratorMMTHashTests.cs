using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMMTHashTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGeneratorMMTHash(GetRandomMock().Object, GetSHAMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should not be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedHash()
        {
            var sha = GetSHAMock();
            sha.Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorMMTHash(GetRandomMock().Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionHash()
        {
            var sha = GetSHAMock();
            sha.Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGeneratorMMTHash(GetRandomMock().Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeHash = new BitString(new byte[] { 1 });
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));

            var sha = GetSHAMock();
            sha
                .Setup(s => s.HashMessage(It.IsAny<BitString>()))
                .Returns(new HashResult(fakeHash));

            var subject = new TestCaseGeneratorMMTHash(random.Object, sha.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success);
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase);

            Assert.IsNotEmpty(((TestCase)result.TestCase).Digest.ToString(), "Digest");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Message.ToString(), "Message");

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
