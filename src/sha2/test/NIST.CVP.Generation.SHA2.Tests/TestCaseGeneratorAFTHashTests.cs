using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAFTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new SHA());
            var result = subject.Generate(new TestGroup { Function = ModeValues.SHA2, DigestSize = DigestSizes.d224 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 1024)]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCallForAllModes(ModeValues function, DigestSizes digestSize, int blockSize)
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = function,
                        DigestSize = digestSize,
                        BitOriented = true,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= blockSize)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                else
                {
                    Assert.AreEqual(blockSize + (99 * longMessageCtr), testCase.Message.BitLength);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 512)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 1024)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 1024)]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCallForAllModes(ModeValues function, DigestSizes digestSize, int blockSize)
        {
            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = function,
                        DigestSize = digestSize,
                        BitOriented = false,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= blockSize / 8)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                else
                {
                    Assert.AreEqual(blockSize + (8 * 99 * longMessageCtr), testCase.Message.BitLength);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        public void ShouldGenerateProperlySizedBitOrientedMessageWhenIncludingNull()
        {
            var function = ModeValues.SHA2;
            var digestSize = DigestSizes.d224;
            var blockSize = 512;

            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = function,
                        DigestSize = digestSize,
                        BitOriented = true,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= blockSize + 1)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                else
                {
                    Assert.AreEqual(blockSize + (99 * longMessageCtr), testCase.Message.BitLength);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        public void ShouldGenerateProperlySizedByteOrientedMessageWhenIncludingNull()
        {
            var function = ModeValues.SHA2;
            var digestSize = DigestSizes.d224;
            var blockSize = 512;

            var mockSHA = new Mock<ISHA>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = function,
                        DigestSize = digestSize,
                        BitOriented = false,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= blockSize / 8 + 1)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                else
                {
                    Assert.AreEqual(blockSize + (8 * 99 * longMessageCtr), testCase.Message.BitLength);
                    longMessageCtr++;
                }
            }
        }
    }
}
