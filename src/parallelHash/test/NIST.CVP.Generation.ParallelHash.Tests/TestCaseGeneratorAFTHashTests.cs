using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAFTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new Crypto.ParallelHash.ParallelHash());
            var result = subject.Generate(new TestGroup { Function = "ParallelHash", DigestSize = 128, OutputLength = domain },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = false,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate * 2)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 1), testCase.Message.BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344, false)]
        [TestCase(256, 1088, false)]
        public void ShouldGenerateProperlySizedCustomizationStringForEachGenerateCall(int digestSize, int rate, bool includeNull)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 1;
            var longMessageCtr = 1;
            var customizationCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate * 2)
                {
                    Assert.AreEqual(customizationCtr, testCase.Customization.Length);
                    customizationCtr = (customizationCtr + 1) % 100;
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    Assert.AreEqual(customizationCtr * longMessageCtr < 2000 ? customizationCtr++ * longMessageCtr : 0, testCase.Customization.Length);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        [TestCase(128, 1344, false)]
        [TestCase(256, 1088, false)]
        public void ShouldGenerateProperlySizedCustomizationHexForEachGenerateCall(int digestSize, int rate, bool includeNull)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 1;
            var longMessageCtr = 1;
            var customizationCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull,
                        OutputLength = domain,
                        HexCustomization = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate * 2)
                {
                    Assert.AreEqual(customizationCtr * 8, testCase.CustomizationHex.BitLength);
                    customizationCtr = (customizationCtr + 1) % 100;
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    Assert.AreEqual(customizationCtr * longMessageCtr < 2000 ? customizationCtr++ * longMessageCtr * 8 : 0, testCase.CustomizationHex.BitLength);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        [TestCase(128, 1344, false)]
        [TestCase(256, 1088, false)]
        public void ShouldGenerateProperBlockSizeForEachGenerateCall(int digestSize, int rate, bool includeNull)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 1;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate * 2)
                {
                    Assert.AreEqual(8, testCase.BlockSize);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    if (longMessageCtr < 33)
                    {
                        Assert.AreEqual(longMessageCtr, testCase.BlockSize);
                    }
                    else if (longMessageCtr < 56)
                    {
                        Assert.AreEqual(64, testCase.BlockSize);
                    }
                    else if (longMessageCtr < 79)
                    {
                        Assert.AreEqual(128, testCase.BlockSize);
                    }
                    else
                    {
                        Assert.AreEqual(256, testCase.BlockSize);
                    }
                    longMessageCtr++;
                }
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = false,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate / 8) * 2)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 8), testCase.Message.BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForBitOrientedMessages(int digestSize, int rate)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = true,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate * 2) + 1)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual((rate + longMessageCtr * (rate + 1)), testCase.Message.BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForByteOrientedMessages(int digestSize, int rate)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = true,
                        OutputLength = domain
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate / 8) * 2 + 1)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Message.BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual((rate + longMessageCtr * (rate + 8)), testCase.Message.BitLength);
                }
            }
        }

        [Test]
        [TestCase(16)]
        [TestCase(679)]
        [TestCase(1601)]
        [TestCase(65535)]
        public void FirstTestShouldHaveMinimumLength(int minLength)
        {
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), minLength, 65536));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new Crypto.ParallelHash.ParallelHash());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    OutputLength = domain
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.AreEqual(minLength, sizeList.First());
        }

        [Test]
        [TestCase(17)]
        [TestCase(679)]
        [TestCase(1601)]
        [TestCase(65536)]
        public void LastTestShouldHaveMaximumLength(int maxLength)
        {
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, maxLength));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new Crypto.ParallelHash.ParallelHash());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    OutputLength = domain
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.AreEqual(maxLength, sizeList.Last());
        }

        [Test]
        [TestCase(16, 65536, true)]
        [TestCase(16, 65536, false)]
        [TestCase(16, 17, true)]
        [TestCase(65535, 65536, true)]
        [TestCase(4000, 6000, false)]
        [TestCase(128, 512, false)]
        [TestCase(128, 256, true)]
        [TestCase(256, 512, false)]
        [TestCase(5679, 12409, true)]
        public void ShouldHaveApproximately1000Tests(int min, int max, bool bitOriented)
        {
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), min, max, bitOriented ? 1 : 8));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new Crypto.ParallelHash.ParallelHash());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 128,
                    BitOrientedOutput = bitOriented,
                    OutputLength = domain
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.GreaterOrEqual(sizeList.Count, 800);
            Assert.LessOrEqual(sizeList.Count, 1200);
        }

        [Test]
        public void ShouldGenerateFullCases()
        {
            var algo = new Mock<IParallelHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), algo.Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = 128,
                        BitOrientedOutput = true,
                        OutputLength = domain
                    }, false);

                Assert.IsTrue(result.Success);
            }
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<IParallelHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 128,
                    OutputLength = domain,
                    BitOrientedInput = false
                }, false);

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldNotModifyTestGroup()
        {
            var algo = new Mock<IParallelHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), algo.Object);
            var testGroup = new TestGroup
            {
                Function = "parallelhash",
                DigestSize = 128,
                BitOrientedOutput = true,
                OutputLength = domain
            };

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);

                Assert.IsTrue(result.Success);
                Assert.AreEqual("parallelhash", testGroup.Function);
                Assert.AreEqual(128, testGroup.DigestSize);
                Assert.AreEqual(true, testGroup.BitOrientedOutput);
                Assert.AreEqual(16, testGroup.OutputLength.GetDomainMinMax().Minimum);
                Assert.AreEqual(65536, testGroup.OutputLength.GetDomainMinMax().Maximum);
            }
        }
    }
}
