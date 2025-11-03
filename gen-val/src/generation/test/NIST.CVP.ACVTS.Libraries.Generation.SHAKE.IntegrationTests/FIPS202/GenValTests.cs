using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.IntegrationTests.FIPS202
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsShake128 : Shake128TestBase
    {
        public override string Algorithm { get; } = "SHAKE-128";
        public override string Revision { get; set; } = "FIPS202";
        public override AlgoMode AlgoMode => AlgoMode.SHAKE_128_FIPS202;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsShake256 : Shake256TestBase
    {
        public override string Algorithm { get; } = "SHAKE-256";
        public override string Revision { get; set; } = "FIPS202";
        public override AlgoMode AlgoMode => AlgoMode.SHAKE_256_FIPS202;
    }
}
