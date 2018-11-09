using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.Core.Async;
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
        public async Task ShouldFailIfNoPlainTextIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PlainText = null;

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains($"{suppliedTestCase.PlainText} was not present"));
        }

        [Test]
        public async Task ShouldFailIfDeferredResolverFails()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult("fail")));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Server unable to complete"));
        }

        [Test]
        public async Task ShouldFailIfPlainTextDoesNotMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.PlainText = new BitString("BEEFFACEBEEFFACE");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(GetTestCase().PlainText, GetFakeIVs())));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains("Plain Text does not match"));
        }

        [Test]
        public async Task ShouldReportSuccessOnPlainTextFirstBlockMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("00000000000000000000000000000000");
            suppliedResult.PlainText = new BitString("0000000000000000BEEFFACEBEEFFACE");
            var fakeTestCase = GetTestCase();
            fakeTestCase.CipherText = new BitString("00000000000000000000000000000000");
            fakeTestCase.PlainText = new BitString("00000000000000000000000000000000");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(fakeTestCase.PlainText, GetFakeIVs())));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), fakeTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public async Task ShouldRunDeferredResolverIfAllComponentsAreInPlace()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(GetTestCase().PlainText, GetFakeIVs())));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.IsNotNull(result);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsRepeated()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2)
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    new BitString(64),
                    new BitString(64)
                })));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("distinct"), "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsOverflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Ones(64),
                    BitString.Zeroes(64)
                })));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("overflow"), "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsUnderflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2)
            };

            var group = new TestGroup
            {
                IncrementalCounter = false,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Ones(64)
                })));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("underflow"), "Reason");
        }

        [Test]
        public async Task ShouldFailWhenNoOverflowOccursWhenItShould()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 2,
                PlainText = new BitString(64 * 2),
                CipherText = new BitString(64 * 2)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Ones(64)
                })));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("none occurred"), "Reason");
        }

        [Test]
        public async Task ShouldFailWhenIvsAreNotSequential()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 4,
                PlainText = new BitString(64 * 4),
                CipherText = new BitString(64 * 4)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(64),
                    BitString.Zeroes(32).ConcatenateBits(BitString.Ones(32)),
                    BitString.Zeroes(40).ConcatenateBits(BitString.Ones(24)),
                    BitString.Ones(64)
                })));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Failed, result.Result, "Result");
            Assert.IsTrue(result.Reason.Contains("greater"), "Reason");
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCaseWithMultipleIvs()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLength = 64 * 6,
                PlainText = new BitString(64 * 6),
                CipherText = new BitString(64 * 6)
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.PlainText, new List<BitString>
                {
                    BitString.Zeroes(32).ConcatenateBits(BitString.Ones(32)),
                    BitString.Zeroes(24).ConcatenateBits(BitString.Ones(40)),
                    BitString.Zeroes(14).ConcatenateBits(BitString.Ones(50)),
                    BitString.Ones(64),
                    BitString.Zeroes(64),
                    BitString.Zeroes(63).ConcatenateBits(BitString.Ones(1))
                })));

            var subject = new TestCaseValidatorCounterDecrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result, "Result");
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCase()
        {
            var goodTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(goodTestCase.PlainText, GetFakeIVs())));

            var subject = new TestCaseValidatorCounterDecrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(goodTestCase);

            Assert.IsNotNull(result);
            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(64),
                Key = new BitString(64),
                PayloadLength = 64,
                CipherText = new BitString(64)
            };
        }

        private List<BitString> GetFakeIVs()
        {
            return new List<BitString>
                {
                new BitString(64)
                };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, SymmetricCounterResult>> GetDeferredResolver()
        {
            return new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, SymmetricCounterResult>>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
