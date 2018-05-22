using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;

namespace NIST.CVP.Generation.RSA_DPComponent.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        private readonly int _casesPerGroup = 10;

        [Test]
        public void ShouldRunVerifyMethodAndFailWithMismatchingNumberOfCases()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = subject.Validate(GetTestCase(6));

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Improper number of replies"), $"Mismatching number of cases. Reason actually was {result.Reason}");
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWhenNotEnoughFailingCases()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = subject.Validate(GetTestCase(_casesPerGroup, 3));

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Incorrect number of failures"), $"Mismatching number of failures. Reason actually was {result.Reason}");
        }

        [Test]
        public void ShouldRunVerifyMethodAndSucceed()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = subject.Validate(GetTestCase(_casesPerGroup));

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        public void ShouldContainReasonWhenCipherTextsDoNotMatch()
        {
            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new ManyEncryptionResult(GetIncorrectResultsArrayListWithBadCipherText(_casesPerGroup)));

            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), mock.Object);
            var result = subject.Validate(GetTestCase(_casesPerGroup));

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Computed cipherText"), $"Reason should contain computed ciphertext did not match. Reason actually was {result.Reason}");
        }

        [Test]
        public void ShouldContainReasonWhenFailureTestsDoNotMatch()
        {
            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new ManyEncryptionResult(GetIncorrectResultsArrayListWithBadFailureTest(_casesPerGroup)));

            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), mock.Object);
            var result = subject.Validate(GetTestCase(_casesPerGroup));

            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Test case should have failed"), $"Reason should contain failure test. Reason actually was {result.Reason}");
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>> GetResolverMock(int totalCount)
        {
            var mock = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new ManyEncryptionResult(GetResultsArrayList(totalCount)));

            return mock;
        }

        private TestCase GetTestCase(int totalCount, int oneFailingOutOf = 2)
        {
            return new TestCase
            {
                TestCaseId = 1,
                ResultsArray = GetResultsArrayList(totalCount, oneFailingOutOf)
            };
        }

        private List<AlgoArrayResponseSignature> GetResultsArrayList(int count, int failing = 2)
        {
            var resultsArray = new List<AlgoArrayResponseSignature>();
            for (var i = 0; i < count; i++)
            {
                resultsArray.Add(new AlgoArrayResponseSignature
                {
                    CipherText = new BitString("ABCD"),
                    Key = new KeyPair {PubKey = new PublicKey{N = 10}},
                    FailureTest = (i % failing == 0),
                    PlainText =  (i % failing != 0) ? new BitString("BCAD") : null
                });
            }

            return resultsArray;
        }

        private List<AlgoArrayResponseSignature> GetIncorrectResultsArrayListWithBadCipherText(int count, int failing = 2)
        {
            var resultsArray = new List<AlgoArrayResponseSignature>();
            for (var i = 0; i < count; i++)
            {
                resultsArray.Add(new AlgoArrayResponseSignature
                {
                    CipherText = new BitString("4321"),
                    Key = new KeyPair {PubKey = new PublicKey{N = 10}},
                    FailureTest = (i % failing == 0),
                    PlainText =  (i % failing != 0) ? new BitString("BCAD") : null
                });
            }

            return resultsArray;
        }

        private List<AlgoArrayResponseSignature> GetIncorrectResultsArrayListWithBadFailureTest(int count, int failing = 2)
        {
            var resultsArray = new List<AlgoArrayResponseSignature>();
            for (var i = 0; i < count; i++)
            {
                resultsArray.Add(new AlgoArrayResponseSignature
                {
                    CipherText = new BitString("4321"),
                    Key = new KeyPair {PubKey = new PublicKey{N = 10}},
                    FailureTest = (i % failing != 0),
                    PlainText =  (i % failing == 0) ? new BitString("BCAD") : null
                });
            }

            return resultsArray;
        }

        private TestGroup GetTestGroup(int totalCount)
        {
            return new TestGroup
            {
                Modulo = 2048,
                TotalTestCases = totalCount,
                TotalFailingCases = totalCount / 2,
            };
        }
    }
}
