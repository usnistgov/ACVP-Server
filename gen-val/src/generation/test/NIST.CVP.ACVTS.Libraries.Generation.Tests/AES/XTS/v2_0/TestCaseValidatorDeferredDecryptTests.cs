using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.XTS.v2_0
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorDeferredDecryptTests
    {
        [Test]
        public async Task ShouldFailIfNoTweakIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.I = null;

            var deferredMock = GetDeferredResolver();

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Tweak value was not present"), Is.True);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
        }

        [Test]
        public async Task ShouldFailIfTweakIsNot128Bits()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.I = new BitString("CAFECAFE");

            var deferredMock = GetDeferredResolver();

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Tweak value was not 128 bits"), Is.True);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
        }

        [Test]
        public async Task ShouldFailIfNoPlainTextIsPresent()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PlainText = null;

            var deferredMock = GetDeferredResolver();

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains($"{nameof(suppliedTestCase.PlainText)} was not present"), Is.True);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Never);
        }

        [Test]
        public async Task ShouldFailIfPlainTextDoesNotMatch()
        {
            var suppliedTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AesXtsResult {
                    PlainText = new BitString("00000000"),
                    Key = suppliedTestCase.Key,
                    Iv = suppliedTestCase.I,
                    CipherText = suppliedTestCase.CipherText,
                    SequenceNumber = 0,
            }));

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Plain Text does not match"), Is.True);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldReportSuccessOnValidTestCase()
        {
            var goodTestCase = GetTestCase();

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AesXtsResult {
                    PlainText = goodTestCase.PlainText,
                    Key = goodTestCase.Key,
                    Iv = goodTestCase.I,
                    CipherText = goodTestCase.CipherText,
                    SequenceNumber = 0,
            }));

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), goodTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(goodTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldFailIfPaddedPlainTextDoesNotMatch()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PayloadLen = 29;

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AesXtsResult {
                    PlainText = suppliedTestCase.PlainText.GetMostSignificantBits(29),
                    Key = suppliedTestCase.Key,
                    Iv = suppliedTestCase.I,
                    CipherText = suppliedTestCase.CipherText,
                    SequenceNumber = 0,
            }));

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), GetTestCase(), deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Failed));
            Assert.That(result.Reason.Contains("Plain Text does not match"), Is.True);

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        [Test]
        public async Task ShouldReportSuccessOnValidPaddedTestCase()
        {
            var suppliedTestCase = GetTestCase();
            suppliedTestCase.PlainText = BitString.PadToNextByteBoundry(suppliedTestCase.PlainText.GetMostSignificantBits(29));
            suppliedTestCase.PayloadLen = 29;

            var deferredMock = GetDeferredResolver();
            deferredMock
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()))
                .Returns(Task.FromResult(new AesXtsResult {
                    PlainText = suppliedTestCase.PlainText.GetMostSignificantBits(29),
                    Key = suppliedTestCase.Key,
                    Iv = suppliedTestCase.I,
                    CipherText = suppliedTestCase.CipherText,
                    SequenceNumber = 0,
            }));

            var subject = new TestCaseValidatorDeferredDecrypt(GetTestGroup(), suppliedTestCase, deferredMock.Object);
            var result = await subject.ValidateAsync(suppliedTestCase);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.EqualTo(Disposition.Passed));

            deferredMock
                .Verify(v => v.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(), It.IsAny<TestCase>()), Times.Once);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                PlainText = new BitString("673E1BB5"),
                CipherText = new BitString("323670A4"),
                Key = new BitString("673E1BB573ED48375517251F3F8F885D9140956F99FC48149155F63AA6CA9CCF"),
                DataUnitLen = 32,
                PayloadLen = 32,
                I = new BitString("00000000000000000000000000000000"),
            };
        }

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesXtsResult>> GetDeferredResolver()
        {
            return new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, AesXtsResult>>();
        }

        private TestGroup GetTestGroup()
        {
            return new TestGroup();
        }
    }
}
