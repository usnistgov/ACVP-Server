using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.SHA;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core;
using NLog;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseGeneratorMonteCarloHashTests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Utilities.ConfigureLogging("SHA", true);
        }

        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), new SHA(), false);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224
                }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(false, 100)]
        [TestCase(true, 3)]
        public void ShouldHaveProperNumberOfTestCasesToGenerate(bool isSample, int expected)
        {
            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), new SHA(), isSample);
            Assert.AreEqual(expected, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        [TestCase("99f957f272b0acc5bfaf38c03078c88c9722671af04f7e399f5e4068", "a97f4fb283d066320eb076b11b28160470709043c58e5edd607aa52e")]
        [TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "527039ed731fbf6b01f7f0c771a18a0c1e78448468ee8f1023e2a545")]
        public void ShouldGenerateProperDigestForSuppliedCase(string message, string digest)
        {
            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), new SHA(), false);
            var testCase = new TestCase(message, null);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224
                }, testCase);

            Assume.That(result != null);
            Assume.That(result.Success);

            var resultTestCase = (TestCase) result.TestCase;
            ThisLogger.Debug(resultTestCase.Digest.ToHex());
            Assert.AreEqual(new BitString(digest).ToHex(), resultTestCase.Digest.ToHex());
        }

        [Test]
        [TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "527039ed731fbf6b01f7f0c771a18a0c1e78448468ee8f1023e2a545", 0)]
        [TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "5dcdf0bc5df31e9e94452bbdffdcb0094ef1d31cc2b47fdd299f114b", 1)]
        [TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "84be4fc2f6962a47a65d6556ed56194b1df357d3c6989af3a02fb55e", 10)]
        //[TestCase("9c8a961df3e1b77fbdf46ae9389b6e2cbe559cd08ef5c960cf98ee09", "3842db2b33a26db980d6f62def34efc4e0b802bf3d5b65ae8f12fc04", 99)]
        public void ShouldHaveProperDigestForGivenCase(string message, string digest, int targetCaseId)
        {
            var seedCase = new TestCase(message, null);
            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), new SHA(), seedCase);
            TestCaseGenerateResponse result = null;

            for (var iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224
                    }, false);
            }
    
            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreEqual(new BitString(digest).ToHex(), resultTestCase.Digest.ToHex());
        }

        [Test]
        [TestCase("", "", 1)]
        public void ShouldNotHaveTheProperDigestForDifferentCase(string message, string digest, int targetCaseId)
        {
            var seedCase = new TestCase(message, null);
            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), new SHA(), seedCase);
            TestCaseGenerateResponse result = null;

            for (var iCaseId = 0; iCaseId <= targetCaseId; iCaseId++)
            {
                result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224
                    }, false);
            }

            Assert.IsNotNull(result);
            var resultTestCase = (TestCase)result.TestCase;
            Assert.AreNotEqual(new BitString(digest).ToHex(), resultTestCase.Digest.ToHex());
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var seedCase = new TestCase("123456", null);
            var algo = new Mock<ISHA>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorMonteCarloHash(new Random800_90(), algo.Object, seedCase);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224
                }, false);
            Assert.IsFalse(result.Success);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}
