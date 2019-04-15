using NIST.CVP.Generation.TupleHash.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
{
    [TestFixture, UnitTest]
    public class ParametersTest
    {
        [Test]
        public void ShouldCoverParametersSet()
        {
            var parameters = new Parameters
            {
                Algorithm = "TupleHash",
                XOF = true,
                DigestSizes = new int[5],
                OutputLength = new MathDomain(),
                MessageLength = new MathDomain(),
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
                Algorithm = "TupleHash",
                MessageLength = minMax,
                OutputLength = minMax,
                IsSample = false,
                XOF = true
            };

            Assume.That(parameters != null);
            Assert.AreEqual("TupleHash", parameters.Algorithm);
        }
    }
}
