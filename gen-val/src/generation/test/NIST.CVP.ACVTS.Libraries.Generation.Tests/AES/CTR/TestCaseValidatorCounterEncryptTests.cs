using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorCounterEncryptTests
    {
        [Test]
        public async Task ShouldFailIfNoCipherTextIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.CipherText = null;

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), GetTestCase(), GetDeferredResolver().Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains($"{suppliedTestCase.CipherText} was not present"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfDeferredResolverFails()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult("fail")));

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Server unable to complete"), Is.True);
        }

        [Test]
        public async Task ShouldFailIfCipherTextDoesNotMatch()
        {
            var suppliedResult = GetTestCase();
            suppliedResult.CipherText = new BitString("BEEFFACEBEEFFACEBEEFFACEBEEFFACE");

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(GetTestCase().CipherText, new List<BitString> { new BitString("abcd"), new BitString("abcd") })));

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Cipher Text does not match"), Is.True);
        }

        [Test]
        public async Task ShouldRunDeferredResolverIfAllComponentsAreInPlace()
        {
            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(GetTestCase().CipherText, new List<BitString> { new BitString("abcd"), new BitString("abcd") })));

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.That(result, Is.Not.Null);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldFailWhenAnIVIsRepeated()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                PayloadLength = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256),
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    new BitString(128),
                    new BitString(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("distinct"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIVIsOverflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                PayloadLength = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256),
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    BitString.Ones(128),
                    BitString.Zeroes(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("overflow"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenAnIVIsUnderflowedWhenItShouldNot()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                PayloadLength = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256),
            };

            var group = new TestGroup
            {
                IncrementalCounter = false,
                OverflowCounter = false
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Ones(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(group, suppliedTestCase, deferredMock.Object);
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
                Key = new BitString(128),
                PayloadLength = 256,
                PlainText = new BitString(256),
                CipherText = new BitString(256),
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Ones(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("none occurred"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldFailWhenIVsAreNotSequential()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                PayloadLength = 128 * 4,
                PlainText = new BitString(128 * 4),
                CipherText = new BitString(128 * 4),
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    BitString.Zeroes(128),
                    BitString.Zeroes(64).ConcatenateBits(BitString.Ones(64)),
                    BitString.Zeroes(100).ConcatenateBits(BitString.Ones(28)),
                    BitString.Ones(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed), "Result");
            Assert.That(result.Reason.Contains("greater"), Is.True, "Reason");
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCaseWithMultipleIVs()
        {
            var suppliedTestCase = new TestCase
            {
                Key = new BitString(128),
                PayloadLength = 128 * 6,
                PlainText = new BitString(128 * 6),
                CipherText = new BitString(128 * 6),
            };

            var group = new TestGroup
            {
                IncrementalCounter = true,
                OverflowCounter = true
            };

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(suppliedTestCase.CipherText, new List<BitString>
                {
                    BitString.Zeroes(120).ConcatenateBits(BitString.Ones(8)),
                    BitString.Zeroes(100).ConcatenateBits(BitString.Ones(28)),
                    BitString.Zeroes(64).ConcatenateBits(BitString.Ones(64)),
                    BitString.Ones(128),
                    BitString.Zeroes(128),
                    BitString.Zeroes(127).ConcatenateBits(BitString.Ones(1))
                })));

            var subject = new TestCaseValidatorCounterEncrypt(group, suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed), result.Reason);
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCase()
        {
            var goodTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new SymmetricCounterResult(goodTestCase.CipherText, new List<BitString>
                {
                    new BitString(128)
                })));

            var subject = new TestCaseValidatorCounterEncrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(goodTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString(128),
                Key = new BitString(128),
                PayloadLength = 128,
                CipherText = new BitString(128)
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
