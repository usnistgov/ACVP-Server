using System;
using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using TestCase = NIST.CVP.Generation.KeyWrap.AES.TestCase;
using TestGroup = NIST.CVP.Generation.KeyWrap.AES.TestGroup;

namespace NIST.CVP.Generation.KeyWrap.Tests.AES
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorDecryptTests
    {
        private Mock<IOracle> _oracle;
        private TestCaseGeneratorDecrypt<TestGroup, TestCase> _subject;

        [SetUp]
        public void Setup()
        {
            _oracle = new Mock<IOracle>();

            _subject = new TestCaseGeneratorDecrypt<TestGroup, TestCase>(_oracle.Object);
        }

        [Test]
        public void GenerateShouldReturnTestCaseGenerateResponse()
        {
            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }

        [Test]
        public void GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            _oracle
                .Setup(s => s.GetKeyWrapCase(It.IsAny<KeyWrapParameters>()))
                .Throws(new Exception());

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
        }

        [Test]
        public void GenerateShouldInvokeEncryptionOperation()
        {
            _subject.Generate(GetTestGroup(), true);

            _oracle.Verify(v => v.GetKeyWrapCase(It.IsAny<KeyWrapParameters>()),
                Times.AtLeastOnce,
                "Encrypt should have been invoked"
            );
        }

        [Test]
        public void GenerateShouldReturnFilledTestCaseObjectOnSuccess()
        {
            var key = new BitString("01");
            var plaintext = new BitString("02");
            var ciphertext = new BitString("03");

            _oracle.Setup(s => s.GetKeyWrapCase(It.IsAny<KeyWrapParameters>()))
                .Returns(new KeyWrapResult()
                {
                    Key = key,
                    Plaintext = plaintext,
                    Ciphertext = ciphertext
                });

            var result = _subject.Generate(GetTestGroup(), false);

            Assert.IsTrue(result.Success, $"{nameof(result)} should be successful");
            Assert.IsInstanceOf(typeof(TestCase), result.TestCase, $"{nameof(result.TestCase)} type mismatch");
            Assert.AreEqual(key, result.TestCase.Key, nameof(key));
            Assert.AreEqual(plaintext, result.TestCase.PlainText, nameof(plaintext));
            Assert.AreEqual(ciphertext, result.TestCase.CipherText, nameof(ciphertext));
            Assert.IsFalse(result.TestCase.Deferred, "Deferred");
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
