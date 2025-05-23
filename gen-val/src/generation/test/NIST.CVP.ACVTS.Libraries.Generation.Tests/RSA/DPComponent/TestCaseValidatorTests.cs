﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.DpComponent;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.DPComponent
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        private readonly int _casesPerGroup = 10;

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithMismatchingNumberOfCases()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = await subject.ValidateAsync(GetTestCase(6));

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Improper number of replies"), Is.True, $"Mismatching number of cases. Reason actually was {result.Reason}");
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWhenNotEnoughFailingCases()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = await subject.ValidateAsync(GetTestCase(_casesPerGroup, 3));

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Incorrect number of failures"), Is.True, $"Mismatching number of failures. Reason actually was {result.Reason}");
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndSucceed()
        {
            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), GetResolverMock(_casesPerGroup).Object);
            var result = await subject.ValidateAsync(GetTestCase(_casesPerGroup));

            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        [Test]
        public async Task ShouldContainReasonWhenCipherTextsDoNotMatch()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new ManyEncryptionResult(GetIncorrectResultsArrayListWithBadCipherText(_casesPerGroup))));

            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), mock.Object);
            var result = await subject.ValidateAsync(GetTestCase(_casesPerGroup));

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Computed cipherText"), Is.True, $"Reason should contain computed ciphertext did not match. Reason actually was {result.Reason}");
        }

        [Test]
        public async Task ShouldContainReasonWhenFailureTestsDoNotMatch()
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new ManyEncryptionResult(GetIncorrectResultsArrayListWithBadFailureTest(_casesPerGroup))));

            var subject = new TestCaseValidator(GetTestGroup(_casesPerGroup), GetTestCase(_casesPerGroup), mock.Object);
            var result = await subject.ValidateAsync(GetTestCase(_casesPerGroup));

            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Test case should have failed"), Is.True, $"Reason should contain failure test. Reason actually was {result.Reason}");
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, ManyEncryptionResult>> GetResolverMock(int totalCount)
        {
            var mock = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, ManyEncryptionResult>>();
            mock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new ManyEncryptionResult(GetResultsArrayList(totalCount))));

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
                    Key = new KeyPair { PubKey = new PublicKey { N = 10 } },
                    TestPassed = (i % failing == 0),
                    PlainText = (i % failing != 0) ? new BitString("BCAD") : null
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
                    Key = new KeyPair { PubKey = new PublicKey { N = 10 } },
                    TestPassed = (i % failing == 0),
                    PlainText = (i % failing != 0) ? new BitString("BCAD") : null
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
                    Key = new KeyPair { PubKey = new PublicKey { N = 10 } },
                    TestPassed = (i % failing != 0),
                    PlainText = (i % failing == 0) ? new BitString("BCAD") : null
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
