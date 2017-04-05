using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture]
    public class TestCaseGeneratorSHAKEVOTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    MinOutputLength = 16,
                    MaxOutputLength = 65536
                }, false);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnAnErrorIfAHashFails()
        {
            var algo = new Mock<ISHA3>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult("Fail"));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 256,
                    BitOrientedOutput = false
                }, false);

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(16)]
        [TestCase(679)]
        [TestCase(1601)]
        [TestCase(65535)]
        [TestCase(65536)]
        public void FirstTestShouldHaveMinimumLength(int minLength)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    MinOutputLength = minLength,
                    MaxOutputLength = 65536
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.AreEqual(minLength, sizeList.First());
        }

        [Test]
        [TestCase(16)]
        [TestCase(17)]
        [TestCase(679)]
        [TestCase(1601)]
        [TestCase(65536)]
        public void LastTestShouldHaveMaximumLength(int maxLength)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    MinOutputLength = 16,
                    MaxOutputLength = maxLength
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.AreEqual(maxLength, sizeList.Last());
        }

        [Test]
        [TestCase(16, 65536, true)]
        [TestCase(16, 65536, false)]
        [TestCase(16, 16, true)]
        [TestCase(16, 17, true)]
        [TestCase(65535, 65536, true)]
        [TestCase(65536, 65536, false)]
        [TestCase(4000, 6000, false)]
        [TestCase(128, 512, false)]
        [TestCase(128, 256, true)]
        [TestCase(256, 512, false)]
        [TestCase(5679, 12409, true)]
        public void ShouldHaveApproximately1000Tests(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 128,
                    BitOrientedOutput = bitOriented,
                    MinOutputLength = min,
                    MaxOutputLength = max
                }, false);

            Assume.That(result.Success);
            var sizeList = subject.TestCaseSizes;

            Assert.GreaterOrEqual(sizeList.Count, 800);
            Assert.LessOrEqual(sizeList.Count, 1200);
        }

        [Test]
        public void ShouldGenerateFullCases()
        {
            var algo = new Mock<ISHA3>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), algo.Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "shake",
                        DigestSize = 128,
                        BitOrientedOutput = true,
                        MinOutputLength = 16,
                        MaxOutputLength = 65536
                    }, false);

                Assert.IsTrue(result.Success);
            }
        }
    }
}
