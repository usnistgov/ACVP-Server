using Moq;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseGeneratorShortHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            var result = subject.Generate(new TestGroup {Function = ModeValues.SHA2, DigestSize = DigestSizes.d224},
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = true,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual(caseIdx + 1, testCase.Message.BitLength);
            }
        }

        [Test]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCall()
        {
            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = false,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase) result.TestCase;
                Assert.AreEqual((caseIdx+1) * 8, testCase.Message.BitLength);
            }
        }

        [Test]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForBitOrientedMessages()
        {
            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = true,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual(caseIdx, testCase.Message.BitLength);
            }
        }

        [Test]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForByteOrientedMessages()
        {
            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = false,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual(caseIdx * 8, testCase.Message.BitLength);
            }
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, false, true, 512)]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, false, false, 64)]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, true, true, 513)]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, true, false, 65)]

        [TestCase(ModeValues.SHA2, DigestSizes.d224, false, true, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, false, false, 64)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, true, true, 513)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, true, false, 65)]

        [TestCase(ModeValues.SHA2, DigestSizes.d256, false, true, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, false, false, 64)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, true, true, 513)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, true, false, 65)]

        [TestCase(ModeValues.SHA2, DigestSizes.d384, false, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, false, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, true, true, 1025)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, true, false, 129)]

        [TestCase(ModeValues.SHA2, DigestSizes.d512, false, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, false, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, true, true, 1025)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, true, false, 129)]

        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, false, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, false, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, true, true, 1025)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, true, false, 129)]

        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, false, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, false, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, true, true, 1025)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, true, false, 129)]
        public void ShouldGenerateProperNumberOfTestCasesForDifferentBlockSizes(ModeValues mode, DigestSizes digestSize,
            bool includeNull, bool bitOriented, int expectedCount)
        {
            var testGroup = new TestGroup
            {
                Function = mode,
                DigestSize = digestSize,
                BitOriented = bitOriented,
                IncludeNull = includeNull,
                TestType = "short"
            };

            var subject = new TestCaseGeneratorShortHash(new Random800_90(), new SHA());
            var result = subject.Generate(testGroup, false);
            Assume.That(result != null);
            Assume.That(result.Success);

            Assert.AreEqual(expectedCount, subject.NumberOfTestCasesToGenerate);
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<ISHA>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorShortHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224,
                    BitOriented = false
                }, false);

            Assert.IsFalse(result.Success);
        }
    }
}
