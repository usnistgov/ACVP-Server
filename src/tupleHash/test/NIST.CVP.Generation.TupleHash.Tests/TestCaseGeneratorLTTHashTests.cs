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
    public class TestCaseGeneratorLTTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), new Crypto.TupleHash.TupleHash());
            var result = subject.Generate(new TestGroup { Function = "cSHAKE", DigestSize = 128 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(128, true)]
        [TestCase(256, false)]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCall(int digestSize, bool xof)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));
            
            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), mockSHA.Object);
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

                foreach (var element in testCase.Tuple)
                {
                    Assert.IsTrue(element.BitLength > 0 && element.BitLength < 2049);
                }
                
            }
        }

        [Test]
        [TestCase(128, true, true)]
        [TestCase(256, false, false)]
        public void ShouldGenerateProperlySizedTupleForEachGenerateCall(int digestSize, bool xof, bool includeNull)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var nonEmpty = 1;
            var empty = 0;
            var semiEmpty = 0;
            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull,
                        XOF = xof
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                if (includeNull)
                {
                    if (empty <= 10)
                    {
                        Assert.IsTrue(testCase.TupleLength == empty++);
                    }
                    else if (semiEmpty <= 30)
                    {
                        Assert.IsTrue(testCase.TupleLength == (semiEmpty++ + 6) / 3);
                    }
                    else if (nonEmpty <= 20)
                    {
                        Assert.IsTrue(testCase.TupleLength == nonEmpty++);
                    }
                    else
                    {
                        Assert.IsTrue(testCase.TupleLength == 5 * nonEmpty++);
                    }
                }
                else
                {
                    if (nonEmpty <= 20)
                    {
                        Assert.IsTrue(testCase.TupleLength == nonEmpty++);
                    }
                    else
                    {
                        Assert.IsTrue(testCase.TupleLength == 5 * nonEmpty++);
                    }
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
            
            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), mockSHA.Object);
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

                Assert.AreEqual(10, testCase.Customization.Length);
            }
        }

        [Test]
        [TestCase(128, 1344, true)]
        [TestCase(256, 1088, false)]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate, bool xof)
        {
            var mockSHA = new Mock<ITupleHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = false,
                        XOF = xof
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                foreach (var element in testCase.Tuple)
                {
                    Assert.IsTrue(element.BitLength > 0 && element.BitLength < 2049);
                    Assert.IsTrue(element.BitLength % 8 == 0);
                }

            }
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<ITupleHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<List<BitString>>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorLTTHash(new Random800_90(), algo.Object);
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
