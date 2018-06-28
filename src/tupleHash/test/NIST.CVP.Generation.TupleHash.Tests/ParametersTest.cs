using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
                BitOrientedInput = true,
                IncludeNull = true,
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
                BitOrientedInput = true,
                BitOrientedOutput = false,
                IncludeNull = true,
                OutputLength = minMax,
                IsSample = false,
                XOF = true
            };

            Assume.That(parameters != null);
            Assert.AreEqual("TupleHash", parameters.Algorithm);
        }
    }
}
