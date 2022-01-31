using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ParallelHash
{
    [TestFixture, UnitTest]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "ParallelHash",
                XOF = new[] { true, false },
                DigestSizes = new int[5].ToList(),
                HexCustomization = false,
                MessageLength = new MathDomain(),
                OutputLength = new MathDomain(),
                IsSample = false
            };
            Assert.IsNotNull(parameters);
        }

        [Test]
        public void ShouldCoverParametersGet()
        {
            var minMax = new MathDomain();
            minMax.AddSegment(new RangeDomainSegment(null, 16, 65536));

            var parameters = new Parameters
            {
                Algorithm = "ParallelHash",
                MessageLength = minMax,
                XOF = new[] { true, false },
                DigestSizes = new int[5].ToList(),
                HexCustomization = false,
                OutputLength = minMax,
                IsSample = false
            };

            Assert.That(parameters != null);
            Assert.AreEqual("ParallelHash", parameters.Algorithm);
        }
    }
}
