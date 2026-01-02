using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.IntegrationTests.v2_0
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha3224 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3-224";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA3_224_v2_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha3256 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3-256";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA3_256_v2_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha3384 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3-384";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA3_384_v2_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha3512 : GenValTestsSha3Base
    {
        public override string Algorithm { get; } = "SHA3-512";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA3_512_v2_0;
    }
}
