using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.KeyWrap.v1_0.AES;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KeyWrap.AES
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorEncryptTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorEncrypt<TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();
            _subject = new TestCaseGeneratorEncrypt<TestGroup, TestCase>(_oracle.Object);
        }

        [Test]
        public async Task GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = await _subject.GenerateAsync(GetTestGroup(), false);

            Assert.That(result, Is.Not.Null, $"{nameof(result)} should be null");
            Assert.That(result, Is.InstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>)), $"{nameof(result)} incorrect type");
        }

        [Test]
        public async Task GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            _oracle
                .Setup(s => s.GetKeyWrapCaseAsync(It.IsAny<KeyWrapParameters>()))
                .Throws(new Exception());

            var result = await _subject.GenerateAsync(GetTestGroup(), false);

            Assert.That(result.TestCase, Is.Null, $"{nameof(result.TestCase)} should be null");
            Assert.That(result.Success, Is.False, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public async Task GenerateShouldInvokeEncryptionOperation()
        {
            await _subject.GenerateAsync(GetTestGroup(), true);

            _oracle.Verify(v => v.GetKeyWrapCaseAsync(It.IsAny<KeyWrapParameters>()),
                Times.AtLeastOnce,
                "Encrypt should have been invoked"
            );
        }

        [Test]
        public async Task GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var key = new BitString("01");
            var plaintext = new BitString("02");
            var ciphertext = new BitString("03");

            _oracle.Setup(s => s.GetKeyWrapCaseAsync(It.IsAny<KeyWrapParameters>()))
                .Returns(Task.FromResult(new KeyWrapResult()
                {
                    Key = key,
                    Plaintext = plaintext,
                    Ciphertext = ciphertext
                }));

            var result = await _subject.GenerateAsync(GetTestGroup(), false);

            Assert.That(result.Success, Is.True, $"{nameof(result)} should be successful");
            Assert.That(result.TestCase, Is.InstanceOf(typeof(TestCase)), $"{nameof(result.TestCase)} type mismatch");
            Assert.That(result.TestCase.Key, Is.EqualTo(key), nameof(key));
            Assert.That(result.TestCase.PlainText, Is.EqualTo(plaintext), nameof(plaintext));
            Assert.That(result.TestCase.CipherText, Is.EqualTo(ciphertext), nameof(ciphertext));
            Assert.That(result.TestCase.Deferred, Is.False, "Deferred");
        }

        private TestGroup GetTestGroup()
        {
            var tg = new TestGroup
            {
                KwCipher = ""
            };

            return tg;
        }
    }
}
