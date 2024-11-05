using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.SP800_56Br2.DpComponent;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SP800_56Br2
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
            var result = await subject.ValidateAsync(GetTestCase(caseNum));

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldRunVerifyMethodAndFailWithNullPlaintext(int caseNum)
        {
            var expectedResult = GetTestCase(caseNum);
            var subject = new TestCaseValidator(expectedResult);
            var failureCase = GetTestCase(caseNum);
            failureCase.PlainText = null;
            
            var result = await subject.ValidateAsync(failureCase);

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("was not present in the"), Is.True, $"IUT Plaintext is null.");
        }
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldRunVerifyMethodAndFailWithMismatchingPlaintexts(int caseNum)
        {
            var expectedResult = GetTestCase(caseNum);
            var subject = new TestCaseValidator(expectedResult);
            var failureCase = GetTestCase(caseNum);
            failureCase.PlainText = new BitString("0000");
            
            var result = await subject.ValidateAsync(failureCase);

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("PlainText does not match expected value"), Is.True, $"PlainText does not match expected value");
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, RsaDecryptionPrimitiveResult>> GetResolverMock(RsaDecryptionPrimitiveResult resultToFake)
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, RsaDecryptionPrimitiveResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(resultToFake));

            return mock;
        }

        private TestCase GetTestCase(int caseNum)
        {
            var tc = new TestCase();
            tc.PlainText = new BitString("ABCD");
            tc.CipherText = new BitString("1234");
            tc.TestPassed = true;
            tc.Deferred = true;
            tc.TestCaseId = caseNum;
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
