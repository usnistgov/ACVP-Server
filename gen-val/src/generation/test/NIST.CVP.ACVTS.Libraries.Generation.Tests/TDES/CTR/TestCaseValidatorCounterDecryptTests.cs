using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains($"{suppliedTestCase.PlainText} was not present"), Is.True);
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Server unable to complete"), Is.True);
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Plain Text does not match"), Is.True);
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed), result.Reason);
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

            Assert.That(result, Is.Not.Null);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsRepeated()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 2,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("distinct"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsOverflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 2,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("overflow"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIvIsUnderflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 2,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("underflow"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenNoOverflowOccursWhenItShould()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 2,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("none occurred"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenIvsAreNotSequential()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 4,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("greater"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCaseWithMultipleIvs()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(64),
                PayloadLen = 64 * 6,
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed), "Result");
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(64),
                Key = new BitString(64),
                PayloadLen = 64,
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
