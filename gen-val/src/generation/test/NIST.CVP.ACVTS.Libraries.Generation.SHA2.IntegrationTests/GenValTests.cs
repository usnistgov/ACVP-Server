using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2OldAlgo224 : GenValTestsSha2OldAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-224";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_224_v1_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2NewAlgo256 : GenValTestsSha2NewAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-256";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_256_v1_0;
    }

    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2OldAlgo384 : GenValTestsSha2OldAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-384";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_384_v1_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2NewAlgo512 : GenValTestsSha2NewAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-512";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_512_v1_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2OldAlgo512224 : GenValTestsSha2OldAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-512/224";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_512_224_v1_0;
    }
    
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTestsSha2NewAlgo512256 : GenValTestsSha2NewAlgoBase
    {
        public override string Algorithm { get; } = "SHA2-512/256";
        public override string[] Modes { get; }
        public override int[] SeedLength { get; }
        public override AlgoMode AlgoMode => AlgoMode.SHA2_512_256_v1_0;
    }
}
