using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
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
        public void ShouldFailIfDeferredResolverFails()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult("fail"));

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
            suppliedResult.PlainText = new BitString("BEEFFACEBEEFFACEBEEFFACEBEEFFACE");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(GetTestCase().PlainText, GetFakeIVs()));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text does not match"));
        }

        [Test]
        public void ShouldReportSuccessOnPlainTextFirstBlockMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("0000000000000000000000000000000000000000000000000000000000000000");
            suppliedResult.PlainText = new BitString("00000000000000000000000000000000BEEFFACEBEEFFACEBEEFFACEBEEFFACE");
            var fakeTestCase = GetTestCase();
            fakeTestCase.CipherText = new BitString("0000000000000000000000000000000000000000000000000000000000000000");
            fakeTestCase.PlainText = new BitString("0000000000000000000000000000000000000000000000000000000000000000");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(fakeTestCase.PlainText, GetFakeIVs()));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), fakeTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public void ShouldRunDeferredResolverIfAllComponentsAreInPlace()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(GetTestCase().PlainText, GetFakeIVs()));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = subject.Validate(GetTestCase());

            Assert.IsNotNull(result);

            deferredMock
                .Verify(v => v.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public void ShouldFailWhenAnIVIsRepeated()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                Length = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256)
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    new BitString(128),
                    new BitString(128)
                }));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("distinct"), "Reason");
        }

        [Test]
        public void ShouldFailWhenAnIVIsOverflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                Length = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Ones(128),
                    BitString.Zeroes(128)
                }));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("overflow"), "Reason");
        }

        [Test]
        public void ShouldFailWhenAnIVIsUnderflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                Length = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256)
            };

            var group = new TestGroup
            {
                IncrementalCounter = false,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Ones(128)
                }));

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
                Key = new BitString(128),
                Length = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Ones(128)
                }));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("none occurred"), "Reason");
        }

        [Test]
        public void ShouldFailWhenIVsAreNotSequential()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                Length = 128 * 4,
                PlainText = new BitString(128 * 4),
                CipherText = new BitString(128 * 4)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Zeroes(64).ConcatenateBits(BitString.Ones(64)),
                    BitString.Zeroes(100).ConcatenateBits(BitString.Ones(28)),
                    BitString.Ones(128)
                }));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = subject.Validate(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("greater"), "Reason");
        }

        [Test]
        public void ShouldReportSuccessOnValidTestCaseWithMultipleIVs()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                Length = 128 * 6,
                PlainText = new BitString(128 * 6),
                CipherText = new BitString(128 * 6)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCrypto(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(120).ConcatenateBits(BitString.Ones(8)),
                    BitString.Zeroes(100).ConcatenateBits(BitString.Ones(28)),
                    BitString.Zeroes(64).ConcatenateBits(BitString.Ones(64)),
                    BitString.Ones(128),
                    BitString.Zeroes(128),
                    BitString.Zeroes(127).ConcatenateBits(BitString.Ones(1))
                }));

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
                .Returns(new SymmetricCounterResult(goodTestCase.PlainText, GetFakeIVs()));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = subject.Validate(goodTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(128),
                Key = new BitString(128),
                Length = 128,
                CipherText = new BitString(128)
            };
        }

        private List<BitString> GetFakeIVs()
        {
            return new List<BitString>
                {
                    new BitString(128)
                };
        }

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>> GetDeferredResolver()
        {
            return new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
