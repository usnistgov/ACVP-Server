using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.TupleHash;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAFTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), new Crypto.TupleHash.TupleHash());
            var result = subject.Generate(new TestGroup { Function = "cSHAKE", DigestSize = 128 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(128, 1344, true)]
        [TestCase(256, 1088, false)]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate, bool xof)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = false,
                        XOF = xof
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate * 2)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Tuple.ElementAt(0).BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 1), testCase.Tuple.ElementAt(0).BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344, false)]
        [TestCase(256, 1088, true)]
        public void ShouldGenerateProperlySizedCustomizationStringForEachGenerateCall(int digestSize, int rate, bool includeNull)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var stringCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull,
                        XOF = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                Assert.AreEqual(stringCtr++, testCase.Customization.Length);
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = false,
                        XOF = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate / 8) * 2)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Tuple.ElementAt(0).BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 8), testCase.Tuple.ElementAt(0).BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForBitOrientedMessages(int digestSize, int rate)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = true,
                        XOF = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate * 2) + 1)
                {
                    Assert.AreEqual(shortMessageCtr, testCase.Tuple.ElementAt(0).BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 1), testCase.Tuple.ElementAt(0).BitLength);
                }
            }
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlyWhenIncludingNullMessageForByteOrientedMessages(int digestSize, int rate)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = true,
                        XOF = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= (rate / 8) * 2 + 1)
                {
                    Assert.AreEqual(shortMessageCtr * 8, testCase.Tuple.ElementAt(0).BitLength);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    longMessageCtr++;
                    Assert.AreEqual(rate + longMessageCtr * (rate + 8), testCase.Tuple.ElementAt(0).BitLength);
                }
            }
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<ITupleHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorAFTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "TupleHash",
                    DigestSize = 128,
                    BitOrientedInput = false,
                    XOF = true
                }, false);

            Assert.IsFalse(result.Success);
        }
    }
}
