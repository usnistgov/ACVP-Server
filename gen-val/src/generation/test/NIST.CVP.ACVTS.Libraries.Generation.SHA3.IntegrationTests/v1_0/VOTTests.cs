using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Builders;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v1_0
{
    [TestFixture, LongRunningIntegrationTest]
    public class VOTTests
    {
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
        public void ShouldGenerateSHAKEVOTWithProperSizes(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorVot(new OracleBuilder().Build().GetAwaiter().GetResult());

            var domain = new MathDomain();
            domain.AddSegment(new RangeDomainSegment(new Random800_90(), min, max, bitOriented ? 1 : 8));

            var minMax = domain.GetDomainMinMax();
            Assert.IsTrue(minMax.Minimum == min, "min");
            Assert.IsTrue(minMax.Maximum == max, "max");

            var group = new TestGroup
            {
                Function = ModeValues.SHAKE,
                DigestSize = DigestSizes.d128,
                BitOrientedOutput = bitOriented,
                OutputLength = domain
            };

            var prepResult = subject.PrepareGenerator(group, false);
            Assert.IsTrue(prepResult.Success);

            var lengths = new List<int>();

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var result = subject.GenerateAsync(group, false).Result;

                Assert.That(result.Success);

                var testCase = result.TestCase;
                lengths.Add(testCase.DigestLength);
                Assert.IsTrue(domain.IsWithinDomain(testCase.DigestLength), $"length: {testCase.DigestLength}");
                Assert.Pass();
            }

            Assert.IsTrue(lengths.Distinct().Count() != 1);
        }
    }
}
