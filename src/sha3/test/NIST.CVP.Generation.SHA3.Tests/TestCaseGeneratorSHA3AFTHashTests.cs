﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSHA3AFTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var result = subject.Generate(new TestGroup { Function = "SHA3", DigestSize = 224 },
                false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(224, 1152)]
        [TestCase(256, 1088)]
        [TestCase(384, 832)]
        [TestCase(512, 576)]
        public void ShouldGenerateProperlySizedBitOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<ISHA3>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "SHA3",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate)
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
        [TestCase(224, 1152)]
        [TestCase(256, 1088)]
        [TestCase(384, 832)]
        [TestCase(512, 576)]
        public void ShouldGenerateProperlySizedByteOrientedMessageForEachGenerateCallForEachRate(int digestSize, int rate)
        {
            var mockSHA = new Mock<ISHA3>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 1;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "SHA3",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = false
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate / 8)
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
        public void ShouldGenerateProperlyWhenIncludingNullMessageForBitOrientedMessages()
        {
            var digestSize = 224;
            var rate = 1152;

            var mockSHA = new Mock<ISHA3>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "SHA3",
                        DigestSize = digestSize,
                        BitOrientedInput = true,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate + 1)
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
        public void ShouldGenerateProperlyWhenIncludingNullMessageForByteOrientedMessages()
        {
            var digestSize = 224;
            var rate = 1152;

            var mockSHA = new Mock<ISHA3>();
            mockSHA.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var shortMessageCtr = 0;
            var longMessageCtr = 0;
            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), mockSHA.Object);
            for (var caseIdx = 0; caseIdx < subject.NumberOfTestCasesToGenerate; caseIdx++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "SHA3",
                        DigestSize = digestSize,
                        BitOrientedInput = false,
                        IncludeNull = true
                    }, false);

                Assume.That(result != null);
                Assume.That(result.Success);

                var testCase = (TestCase)result.TestCase;

                // Short message
                if (shortMessageCtr <= rate / 8 + 1)
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
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<ISHA3>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorSHA3AFTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "SHA3",
                    DigestSize = 512,
                    BitOrientedInput = false
                }, false);

            Assert.IsFalse(result.Success);
        }
    }
}