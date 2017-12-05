using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.CTR;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorCounterDecryptTests
    {
        [Test]
        public void ShouldFailIfNoPlainTextIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PlainText = null;

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{suppliedTestCase.PlainText} was not present"));
        }

        [Test]
        public void ShouldFailIfNoIvsArePresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.Ivs = null;

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedTestCase.Ivs)} was not present"));
        }

        [Test]
        public void ShouldFailWithIncorrectNumberOfIvs()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.Ivs = new List<BitString>();

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedTestCase.Ivs)} does not have the correct number"), "Reason");
        }

        [Test]
        public void ShouldFailIfDeferredResolverFails()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult("fail"));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(GetTestCase());

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Server unable to complete"));
        }

        [Test]
        public void ShouldFailIfPlainTextDoesNotMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("BEEFFACE");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(GetTestCase().PlainText, GetTestCase().Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text does not match"));
        }

        [Test]
        public void ShouldRunDeferredResolverIfAllComponentsAreInPlace()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(GetTestCase().PlainText, GetTestCase().Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(GetTestCase());

            Assert.IsNotNull(result);

            deferredMock
                .Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public void ShouldFailWhenAnIvIsRepeated()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2),
                Ivs = new List<BitString>
                {
                    new BitString(64),
                    new BitString(64)
                }
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("distinct"), "Reason");
        }

        [Test]
        public void ShouldFailWhenAnIvIsOverflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2),
                Ivs = new List<BitString>
                {
                    BitString.Ones(64),
                    BitString.Zeroes(64)
                }
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("overflow"), "Reason");
        }

        [Test]
        public void ShouldFailWhenAnIvIsUnderflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2),
                Ivs = new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Ones(64)
                }
            };

            var group = new TestGroup
            {
                IncrementalCounter = false,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("underflow"), "Reason");
        }

        [Test]
        public void ShouldFailWhenNoOverflowOccursWhenItShould()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2),
                Ivs = new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Ones(64)
                }
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("none occurred"), "Reason");
        }

        [Test]
        public void ShouldFailWhenIvsAreNotSequential()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 4,
                PlainText = new BitString(64 * 4),
                CipherText = new BitString(64 * 4),
                Ivs = new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Zeroes(32).ConcatenateBits(BitString.Ones(32)),
                    BitString.Zeroes(40).ConcatenateBits(BitString.Ones(24)),
                    BitString.Ones(64)
                }
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("greater"), "Reason");
        }

        [Test]
        public void ShouldReportSuccessOnValidTestCaseWithMultipleIvs()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                Length = 64 * 6,
                PlainText = new BitString(64 * 6),
                CipherText = new BitString(64 * 6),
                Ivs = new List<BitString>
                {
                    BitString.Zeroes(32).ConcatenateBits(BitString.Ones(32)),
                    BitString.Zeroes(24).ConcatenateBits(BitString.Ones(40)),
                    BitString.Zeroes(14).ConcatenateBits(BitString.Ones(50)),
                    BitString.Ones(64),
                    BitString.Zeroes(64),
                    BitString.Zeroes(63).ConcatenateBits(BitString.Ones(1))
                }
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(suppliedTestCase.PlainText, suppliedTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result, "Result");
        }

        [Test]
        public void ShouldReportSuccessOnValidTestCase()
        {
            var goodTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new CounterDecryptionResult(goodTestCase.PlainText, goodTestCase.Ivs));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = subject.Validate(goodTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(64),
                Key = new BitString(64),
                Length = 64,
                Ivs = new List<BitString>
                {
                    new BitString(64)
                },
                CipherText = new BitString(64)
            };
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult>> GetDeferredResolver()
        {
            return new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult>>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
