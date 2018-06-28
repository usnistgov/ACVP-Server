using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorLCTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorLCTHash(new Random800_90(), new Crypto.ParallelHash.ParallelHash());
            var result = subject.Generate(new TestGroup { Function = "ParallelHash", DigestSize = 128 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(128, 1344)]
        [TestCase(256, 1088)]
        public void ShouldGenerateProperlySizedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));
            
            var subject = new TestCaseGeneratorLCTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                Assert.AreEqual(digestSize, testCase.Message.BitLength);
            }
        }

        [Test]
        [TestCase(128, 1344, false)]
        [TestCase(256, 1088, true)]
        public void ShouldGenerateProperlySizedCustomizationStringForEachGenerateCall(int digestSize, int rate, bool includeNull)
        {
            var mockSHA = new Mock<IParallelHash>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var customizationCtr = 1;
            var longMessageCtr = 1;
            var subject = new TestCaseGeneratorLCTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = includeNull
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= 30)
                {
                    Assert.AreEqual(customizationCtr++, testCase.Customization.Length);
                    shortMessageCtr++;
                }
                // Long message
                else
                {
                    Assert.AreEqual(customizationCtr++ * longMessageCtr * longMessageCtr, testCase.Customization.Length);
                    longMessageCtr++;
                }
            }
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<IParallelHash>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorLCTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "ParallelHash",
                    DigestSize = 128,
                    BitOrientedInput = false
                }, false);

            Assert.IsFalse(result.Success);
        }
    }
}
