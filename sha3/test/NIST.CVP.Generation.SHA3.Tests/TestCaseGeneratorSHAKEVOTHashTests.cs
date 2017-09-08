using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorSHAKEVOTHashTests
    {
        [Test]
        public void ShouldSuccessfullyGenerate()
        {
            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 128,
                    BitOrientedOutput = true,
                    OutputLength = domain
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

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), algo.Object);
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
                    DigestSize = 256,
                    BitOrientedOutput = false,
                    OutputLength = domain
                }, false);

            Assert.IsFalse(result.Success);
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

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
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

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
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

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var result = subject.Generate(
                new TestGroup
                {
                    Function = "shake",
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
            var algo = new Mock<ISHA3>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), algo.Object);

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(
                    new TestGroup
                    {
                        Function = "shake",
                        DigestSize = 128,
                        BitOrientedOutput = true,
                        OutputLength = domain
                    }, false);

                Assert.IsTrue(result.Success);
            }
        }

        [Test]
        public void ShouldNotModifyTestGroup()
        {
            var algo = new Mock<ISHA3>();
            algo.Setup(s => s.HashMessage(It.IsAny<HashFunction>(), It.IsAny<BitString>()))
                .Returns(new HashResult(new BitString("ABCD")));

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), 16, 65536));

            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), algo.Object);
            var testGroup = new TestGroup
            {
                Function = "shake",
                DigestSize = 128,
                BitOrientedOutput = true,
                OutputLength = domain
            };

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.Generate(testGroup, false);

                Assert.IsTrue(result.Success);
                Assert.AreEqual("shake", testGroup.Function);
                Assert.AreEqual(128, testGroup.DigestSize);
                Assert.AreEqual(true, testGroup.BitOrientedOutput);
                Assert.AreEqual(16, testGroup.OutputLength.GetDomainMinMax().Minimum);
                Assert.AreEqual(65536, testGroup.OutputLength.GetDomainMinMax().Maximum);
            }
        }
    }
}
