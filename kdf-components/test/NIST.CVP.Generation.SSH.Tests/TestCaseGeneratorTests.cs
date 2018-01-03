using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.SSH.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SSH.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var subject = new TestCaseGenerator(GetRandomMock().Object, GetSshMock().Object);

            var result = subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedSsh()
        {
            var ssh = GetSshMock();
            ssh
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new KdfResult("Fail"));

            var subject = new TestCaseGenerator(GetRandomMock().Object, ssh.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionSsh()
        {
            var ssh = GetSshMock();
            ssh
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Throws(new Exception());

            var subject = new TestCaseGenerator(GetRandomMock().Object, ssh.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeGenerateSshOperation()
        {
            var ssh = GetSshMock();

            var subject = new TestCaseGenerator(GetRandomMock().Object, ssh.Object);

            var result = subject.Generate(GetTestGroup(), true);

            ssh.Verify(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()),
                Times.AtLeastOnce,
                "DeriveKey should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeKey = new BitString(new byte[] { 1 });
            var fakeOneWayResult = new OneWayResult
            {
                EncryptionKey = fakeKey,
                IntegrityKey = fakeKey,
                InitialIv = fakeKey
            };

            var ssh = GetSshMock();
            ssh
                .Setup(s => s.DeriveKey(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new KdfResult(fakeOneWayResult, fakeOneWayResult));

            var subject = new TestCaseGenerator(GetRandomMock().Object, ssh.Object);

            var result = subject.Generate(GetTestGroup(), true);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");

            Assert.IsNotEmpty(((TestCase)result.TestCase).InitialIvServer.ToString(), "InitialIvServer");
            Assert.IsNotEmpty(((TestCase)result.TestCase).EncryptionKeyServer.ToString(), "EncryptionKeyServer");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IntegrityKeyServer.ToString(), "IntegrityKeyServer");

            Assert.IsNotEmpty(((TestCase)result.TestCase).InitialIvClient.ToString(), "InitialIvClient");
            Assert.IsNotEmpty(((TestCase)result.TestCase).EncryptionKeyClient.ToString(), "EncryptionKeyClient");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IntegrityKeyClient.ToString(), "IntegrityKeyClient");
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            var random = new Mock<IRandom800_90>();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(128));
            return random;
        }

        private Mock<ISsh> GetSshMock()
        {
            return new Mock<ISsh>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup
            {
                HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                Cipher = Cipher.AES128,
            };
        }
    }
}
