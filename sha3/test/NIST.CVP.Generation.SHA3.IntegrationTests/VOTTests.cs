using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class VOTTests
    {
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
        public void ShouldGenerateSHAKEVOTWithProperSizes(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
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

                var testCase = (TestCase) result.TestCase;
                Assert.AreEqual(subject.TestCaseSizes[i], testCase.Digest.BitLength);
            }
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
        public void TestCasesShouldNeverExceedMinimumAndMaximum(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
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

                var testCase = (TestCase)result.TestCase;
                Assert.GreaterOrEqual(testCase.Digest.BitLength, min);
                Assert.LessOrEqual(testCase.Digest.BitLength, max);
            }
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
        public void TestCasesShouldConstantlyGrow(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorSHAKEVOTHash(new Random800_90(), new Crypto.SHA3.SHA3());
            var prevCase = 0;

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
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

                var testCase = (TestCase)result.TestCase;

                Assert.GreaterOrEqual(testCase.Digest.BitLength, prevCase);
                prevCase = testCase.Digest.BitLength;
            }
        }
    }
}
