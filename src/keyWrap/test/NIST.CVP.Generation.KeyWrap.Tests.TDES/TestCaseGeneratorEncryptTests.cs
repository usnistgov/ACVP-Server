﻿using System;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KeyWrap.TDES;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KeyWrap.Tests.TDES
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

            Assert.IsNotNull(result, $"{nameof(result)} should be null");
            Assert.IsInstanceOf(typeof(TestCaseGenerateResponse<TestGroup, TestCase>), result, $"{nameof(result)} incorrect type");
        }
        
        [Test]
        public async Task GenerateShouldReturnNullITestCaseOnExceptionEncryption()
        {
            _oracle
                .Setup(s => s.GetKeyWrapCaseAsync(It.IsAny<KeyWrapParameters>()))
                .Throws(new Exception());

            var result = await _subject.GenerateAsync(GetTestGroup(), false);

            Assert.IsNull(result.TestCase, $"{nameof(result.TestCase)} should be null");
            Assert.IsFalse(result.Success, $"{nameof(result.Success)} should indicate failure");
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