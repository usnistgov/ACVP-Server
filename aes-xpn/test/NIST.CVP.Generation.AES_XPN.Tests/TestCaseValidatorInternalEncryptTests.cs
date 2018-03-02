using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_XPN.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorInternalEncryptTests
    {
        private TestCaseValidatorInternalEncrypt _subject;
        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>>();
        }

        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);

            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCipherAeadResult(suppliedResult.CipherText, suppliedResult.Tag));

            var result = _subject.Validate((TestCase)testGroup.Tests[0]);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldFailIfCipherTextDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCipherAeadResult(suppliedResult.CipherText, suppliedResult.Tag));

            suppliedResult.CipherText = new BitString("D00000");

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCipherAeadResult(suppliedResult.CipherText, suppliedResult.Tag));

            suppliedResult.CipherText = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Cipher Text"));
        }

        [Test]
        public void ShouldFailIfTagDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCipherAeadResult(suppliedResult.CipherText, suppliedResult.Tag));

            suppliedResult.Tag = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        [Test]
        public void ShouldShowTagAsReasonIfItDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCipherAeadResult(suppliedResult.CipherText, suppliedResult.Tag));

            suppliedResult.Tag = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains("Tag"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.CipherText = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfTagNotPresent()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.Tag = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Tag)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfIVNotPresentInSuppliedTestCaseWhenInternal()
        {
            var testGroup = GetTestGroup("internal", "external");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.IV = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.IV)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfSaltNotPresentInSuppliedTestCaseWhenInternal()
        {
            var testGroup = GetTestGroup("external", "internal");
            _subject = new TestCaseValidatorInternalEncrypt(testGroup, (TestCase)testGroup.Tests[0], _deferredResolver.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.Salt = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Salt)} was not present in the {nameof(TestCase)}"));
        }

        private TestGroup GetTestGroup(string ivGen, string saltGen)
        {
            TestCase tc = new TestCase()
            {
                AAD = new BitString(128),
                Key = new BitString(128),
                PlainText = new BitString(128),
                CipherText = new BitString(128),
                Tag = new BitString(128),
                IV = new BitString(96),
                Salt = new BitString(96)
        };

            TestGroup tg = new TestGroup()
            {
                SaltGen = saltGen,
                IVGeneration = ivGen,
                Tests = new List<ITestCase>()
                {
                    tc
                }
            };
            return tg;
        }
    }
}
