using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
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
        private Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>> _factory;
        private Mock<ITestCaseGenerator<TestGroup, TestCase>> _generator;

        [SetUp]
        public void Setup()
        {
            _generator = new Mock<ITestCaseGenerator<TestGroup, TestCase>>();
            _generator.Setup(s => s.Generate(It.IsAny<TestGroup>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse((TestCase)GetTestGroup("", "").Tests[0]));
            _factory = new Mock<ITestCaseGeneratorFactory<TestGroup, TestCase>>();
            _factory
                .Setup(s => s.GetCaseGenerator(It.IsAny<TestGroup>()))
                .Returns(_generator.Object);
        }

        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var result = _subject.Validate((TestCase)testGroup.Tests[0]);
            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfCipherTextDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];
            suppliedResult.CipherText = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowCipherTextAsReasonIfItDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];
            suppliedResult.CipherText = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Cipher Text"));
        }

        [Test]
        public void ShouldFailIfTagDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];
            suppliedResult.Tag = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowTagAsReasonIfItDoesNotMatch()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];
            suppliedResult.Tag = new BitString("D00000");
            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Tag"));
        }

        [Test]
        public void ShouldFailIfCipherTextNotPresent()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.CipherText = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfTagNotPresent()
        {
            var testGroup = GetTestGroup("internal", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.Tag = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Tag)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfIVNotPresentInSuppliedTestCaseWhenInternal()
        {
            var testGroup = GetTestGroup("internal", "external");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.IV = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.IV)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldFailIfSaltNotPresentInSuppliedTestCaseWhenInternal()
        {
            var testGroup = GetTestGroup("external", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            suppliedResult.Salt = null;

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.Salt)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public void ShouldReportFailureIfEncryptOperationFails()
        {
            _generator
                .Setup(s => s.Generate(It.IsAny<TestGroup>(), It.IsAny<TestCase>()))
                .Returns(new TestCaseGenerateResponse("fail"));

            var testGroup = GetTestGroup("external", "internal");
            _subject = new TestCaseValidatorInternalEncrypt((TestCase)testGroup.Tests[0], testGroup, _factory.Object);
            var suppliedResult = (TestCase)GetTestGroup("internal", "internal").Tests[0];

            var result = _subject.Validate(suppliedResult);
            Assume.That(result != null);
            Assume.That("failed" == result.Result);

            Assert.IsTrue(result.Reason.Contains("Failed generating TestCase on inputs"));
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
