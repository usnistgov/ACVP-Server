using System.Threading.Tasks;
using Moq;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.AES_GCM.v1_0;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorDeferredEncryptTests
    {
        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>> _mock;
        
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();

            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = testCase.CipherText,
                    Tag = testCase.Tag
                }));
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public async Task ShouldFailIfCipherTextDoesNotMatch()
        {
            var testCase = GetTestCase();
            
            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = new BitString("BADBEEF0"),
                    Tag = testCase.Tag
                }));
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Cipher Text"));
        }

        [Test]
        public async Task ShouldFailIfTagDoesNotMatch()
        {
            var testCase = GetTestCase();
            
            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = testCase.CipherText,
                    Tag = new BitString("BADBEEF0")
                }));
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);
            Assume.That(result != null);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Tag"));
        }

        [Test]
        public async Task ShouldFailIfCipherTextNotPresent()
        {
            var testCase = GetTestCase();
            
            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = testCase.CipherText,
                    Tag = testCase.Tag
                }));

            testCase.CipherText = null;
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);

            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(testCase.CipherText)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public async Task ShouldFailIfTagNotPresent()
        {
            var testCase = GetTestCase();
            
            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = testCase.CipherText,
                    Tag = testCase.Tag
                }));

            testCase.Tag = null;
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);

            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(testCase.Tag)} was not present in the {nameof(TestCase)}"));
        }

        [Test]
        public async Task ShouldFailIfIvNotPresent()
        {
            var testCase = GetTestCase();
            testCase.IV = null;
            
            _mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult>>();
            _mock.Setup(s =>
                    s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AeadResult
                {
                    CipherText = testCase.CipherText,
                    Tag = testCase.Tag
                }));
            
            var subject = new TestCaseValidatorDeferredEncrypt(GetTestGroup(), testCase, _mock.Object);
            var result = await subject.ValidateAsync(testCase);

            Assume.That(result != null);
            Assume.That(Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(testCase.IV)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                Tag = new BitString("AADAADAADAAD"),
                CipherText = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                IV = new BitString("ABCD"),
                TestCaseId = 1
            };
            return testCase;
        }
        
        private TestGroup GetTestGroup(AlgoMode algoMode = AlgoMode.AES_GCM_v1_0)
        {
            return new TestGroup()
            {
                AlgoMode = algoMode
            };
        }
    }
}
