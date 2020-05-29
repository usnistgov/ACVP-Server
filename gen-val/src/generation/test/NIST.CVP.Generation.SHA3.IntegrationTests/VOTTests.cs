using NIST.CVP.Crypto.Oracle.Builders;
using NIST.CVP.Generation.SHA3.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3.IntegrationTests
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
        public async Task ShouldGenerateSHAKEVOTWithProperSizes(int min, int max, bool bitOriented)
        {
            var subject = new TestCaseGeneratorVot(new OracleBuilder().Build().GetAwaiter().GetResult());
            var prevCase = 0;

            for (var i = 0; i < subject.NumberOfTestCasesToGenerate; i++)
            {
                var domain = new MathDomain();
                domain.AddSegment(new RangeDomainSegment(new Random800_90(), min, max, bitOriented ? 1 : 8));

                var result = await subject.GenerateAsync(
                    new TestGroup
                    {
                        Function = "shake",
                        DigestSize = 128,
                        BitOrientedOutput = bitOriented,
                        OutputLength = domain
                    }, false);

                Assume.That(result.Success);

                var testCase = result.TestCase;
                Assert.AreEqual(subject.TestCaseSizes[i], testCase.Digest.BitLength);
                Assert.GreaterOrEqual(testCase.Digest.BitLength, prevCase);
                prevCase = testCase.Digest.BitLength;
            }
        }
    }
}
