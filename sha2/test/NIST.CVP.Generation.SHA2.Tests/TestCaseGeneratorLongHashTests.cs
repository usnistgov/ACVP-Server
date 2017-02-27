using Moq;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
    public class TestCaseGeneratorLongHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorLongHash(new Random800_90(), mockSHA.Object);
            var result = subject.Generate(new TestGroup { Function = ModeValues.SHA2, DigestSize = DigestSizes.d224 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCall()
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorLongHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 99 + 512, testCase.Message.BitLength);
            }
        }

        [Test]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCall()
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorLongHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        BitOriented = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;
                Assert.AreEqual((caseIdx + 1) * 8 * 99 + 512, testCase.Message.BitLength);
            }
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, true, 512)]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, false, 64)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, true, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, false, 64)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, true, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, false, 64)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, false, 128)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, true, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, false, 128)]
        public void ShouldGenerateProperNumberOfTestCasesForDifferentBlockSizes(ModeValues mode, DigestSizes digestSize,
            bool bitOriented, int expectedCount)
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var testGroup = new TestGroup
            {
                Function = mode,
                DigestSize = digestSize,
                BitOriented = bitOriented,
                IncludeNull = false,
                TestType = "long"
            };

            var subject = new TestCaseGeneratorLongHash(new Random800_90(), mockSHA.Object);
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

            var subject = new TestCaseGeneratorLongHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224
                }, false);

            Assert.IsFalse(result.Success);
        }
    }
}
