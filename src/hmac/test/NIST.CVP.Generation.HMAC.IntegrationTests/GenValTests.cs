using NIST.CVP.Common;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha1 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA-1";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA1_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-224";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_224_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-256";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_256_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_384 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-384";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_384_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_512_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512/224";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_512_224_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512/256";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA2_512_256_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-224";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA3_224_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-256";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA3_256_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_384 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-384";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA3_384_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_512 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-512";
        public override AlgoMode AlgoMode => AlgoMode.HMAC_SHA3_512_v1_0;
    }
}
