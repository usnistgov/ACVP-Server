using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SPComponent_v2_0
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        private readonly int _casesPerGroup = 10;

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldRunVerifyMethodAndSucceed(int caseNum)
        {
            var testGroup = GetTestGroup(_casesPerGroup);
            var testCase = GetTestCase(caseNum);

            var subject = new TestCaseValidator(testCase);
            var result = await subject.ValidateAsync(GetTestCase(caseNum, false));

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldRunVerifyMethodAndFailWithNullSignature(int caseNum)
        {
            var expectedResult = GetTestCase(caseNum);
            var subject = new TestCaseValidator(expectedResult);
            var failureCase = GetTestCase(caseNum);
            failureCase.Signature = null;
            
            var result = await subject.ValidateAsync(failureCase);

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("was not present in the"), Is.True, $"IUT Signature is null.");
        }
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldRunVerifyMethodAndFailWithMismatchingSignatures(int caseNum)
        {
            var expectedResult = GetTestCase(caseNum);
            var subject = new TestCaseValidator(expectedResult);
            var failureCase = GetTestCase(caseNum);
            failureCase.Signature = new BitString("0000");
            
            var result = await subject.ValidateAsync(failureCase);

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Signature does not match expected value"), Is.True, $"Signature does not match expected value");
        }

        private RsaSignaturePrimitiveResult GetRsaSignaturePrimitiveResult(int caseNum, bool isCrt = false)
        {
            var result = new RsaSignaturePrimitiveResult();
            var tc = GetTestCase(caseNum, isCrt);
            result.Message = tc.Message;
            result.Signature = tc.Signature;
            result.ShouldPass = true;
            return result;
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, RsaDecryptionPrimitiveResult>> GetResolverMock(RsaDecryptionPrimitiveResult resultToFake)
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, RsaDecryptionPrimitiveResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(resultToFake));

            return mock;
        }

        private TestCase GetTestCase(int caseNum, bool isCrt = false)
        {
            var tc = new TestCase();
            tc.Message = new BitString("ABCD");
            tc.Signature = new BitString("1234");
            tc.Deferred = true;
            tc.TestCaseId = caseNum;
            tc.TestPassed = true;
            return tc;
        }

        private TestGroup GetTestGroup(int testCaseCount, bool isCrt = false)
        {
            return new TestGroup
            {
                Modulo = 2048,
                TestType = "AFT",
                KeyMode = isCrt ? PrivateKeyModes.Crt : PrivateKeyModes.Standard,
                TestGroupId = 1
            };
        }
    }
}
