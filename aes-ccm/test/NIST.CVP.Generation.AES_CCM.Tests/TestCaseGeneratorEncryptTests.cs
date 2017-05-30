using System;
using Moq;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorEncryptTests
    {
        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(GetRandomMock().Object, GetAESMock().Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnFailedEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new EncryptionResult("Fail"));

            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Throws(new Exception());

            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            var aes = GetAESMock();

            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(GetRandomMock().Object, aes.Object);

            var result = subject.Generate(new TestGroup(), true);

            aes.Verify(v => v.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()),
                Times.AtLeastOnce,
                "BlockEncrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var fakeCipher = new BitString(new byte[] { 1 });
            var random = GetRandomMock();
            random
                .Setup(s => s.GetRandomBitString(It.IsAny<int>()))
                .Returns(new BitString(new byte[] { 3 }));
            var aes = GetAESMock();
            aes
                .Setup(s => s.Encrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<BitString>(), It.IsAny<int>()))
                .Returns(new EncryptionResult(fakeCipher));

            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(random.Object, aes.Object);

            var result = subject.Generate(new TestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.IsNotEmpty(((TestCase)result.TestCase).AAD.ToString(), "AAD");
            Assert.IsNotEmpty(((TestCase)result.TestCase).CipherText.ToString(), "CipherText");
            Assert.IsNotEmpty(((TestCase)result.TestCase).IV.ToString(), "IV");
            Assert.IsNotEmpty(((TestCase)result.TestCase).Key.ToString(), "Key");
            Assert.IsNotEmpty(((TestCase)result.TestCase).PlainText.ToString(), "PlainText");
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
        }

        [Test]
        [TestCase("No change 0 bits", 0, 10)]
        [TestCase("No change 10 bits", 10, 10)]
        [TestCase("No change 32 bits", 32, 10)]
        [TestCase("no change 33 bits", 33, 10)]
        [TestCase("no change 256 bits", 256, 10)]
        [TestCase("change 257 bits", 257, 1)]
        [TestCase("change 65536", 65536, 1)]
        public void ShouldChangeNumberOfTestCasesWhenAadLenGt32bytes(string testLabel, int aadLen, int expectedNumberOfCases)
        {
            TestCaseGeneratorEncrypt subject =
                new TestCaseGeneratorEncrypt(GetRandomMock().Object, GetAESMock().Object);

            TestGroup tg = new TestGroup()
            {
                AADLength = aadLen
            };

            var result = subject.Generate(tg, true);

            Assert.AreEqual(expectedNumberOfCases, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReuseKeyForReuseKeyGroup(bool shouldReuse)
        {
            TestGroup tg = new TestGroup()
            {
                GroupReusesKeyForTestCases = shouldReuse,
                AADLength = 32,
                Function = "encrypt",
                IVLength = 13*8,
                KeyLength = 128,
                PTLength = 32,
                TagLength = 16*8
            };

            var aesMock = GetAESMock();
            aesMock
                .Setup(
                    s => s.Encrypt(
                        It.IsAny<BitString>(), 
                        It.IsAny<BitString>(), 
                        It.IsAny<BitString>(), 
                        It.IsAny<BitString>(),
                        It.IsAny<int>()
                    )
               )
               .Returns(new EncryptionResult(new BitString(1)));

            TestCaseGeneratorEncrypt subject = new TestCaseGeneratorEncrypt(new Random800_90(), aesMock.Object);
            var test1 = subject.Generate(tg, true);
            var test2 = subject.Generate(tg, true);

            if (shouldReuse)
            {
                Assert.AreEqual(((TestCase) test1.TestCase).Key, ((TestCase) test2.TestCase).Key);
            }
            else
            {
                Assert.AreNotEqual(((TestCase)test1.TestCase).Key, ((TestCase)test2.TestCase).Key);
            }
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldReuseNonceForReuseNonceGroup(bool shouldReuse)
        {
            TestGroup tg = new TestGroup()
            {
                GroupReusesNonceForTestCases = shouldReuse,
                AADLength = 32,
                Function = "encrypt",
                IVLength = 13 * 8,
                KeyLength = 128,
                PTLength = 32,
                TagLength = 16 * 8
            };

            var aesMock = GetAESMock();
            aesMock
                .Setup(
                    s => s.Encrypt(
                        It.IsAny<BitString>(),
                        It.IsAny<BitString>(),
                        It.IsAny<BitString>(),
                        It.IsAny<BitString>(),
                        It.IsAny<int>()
                    )
               )
               .Returns(new EncryptionResult(new BitString(1)));

            TestCaseGeneratorEncrypt subject = new TestCaseGeneratorEncrypt(new Random800_90(), aesMock.Object);
            var test1 = subject.Generate(tg, true);
            var test2 = subject.Generate(tg, true);

            if (shouldReuse)
            {
                Assert.AreEqual(((TestCase)test1.TestCase).IV, ((TestCase)test2.TestCase).IV);
            }
            else
            {
                Assert.AreNotEqual(((TestCase)test1.TestCase).IV, ((TestCase)test2.TestCase).IV);
            }
        }

        private Mock<IRandom800_90> GetRandomMock()
        {
            return new Mock<IRandom800_90>();
        }

        private Mock<IAES_CCM> GetAESMock()
        {
            return new Mock<IAES_CCM>();
        }
    }
}
