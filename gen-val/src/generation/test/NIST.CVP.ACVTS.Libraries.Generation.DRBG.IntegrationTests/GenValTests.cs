using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.DRBG.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsCtr : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";

        public override string[] Modes => new[]
        {
            "AES-128",
            "AES-192",
            "AES-256",
            "TDES",
        };

        public override int[] SeedLength => new[]
        {
            256,
            320,
            384,
            232
        };

        public override AlgoMode AlgoMode => AlgoMode.DRBG_CTR_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHash : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string[] Modes => new[]
        {
            "SHA-1",
            "SHA2-224",
            "SHA2-256",
            "SHA2-384",
            "SHA2-512",
            "SHA2-512/224",
            "SHA2-512/256",
            "SHA3-224",
            "SHA3-256",
            "SHA3-384",
            "SHA3-512"
        };

        public override int[] SeedLength => new[]
        {
            160,
            224,
            256,
            384,
            512,
            224,
            256,
            224,
            256,
            384,
            512
        };

        public override AlgoMode AlgoMode => AlgoMode.DRBG_Hash_v1_0;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMAC : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";

        public override string[] Modes => new[]
                {
                    "SHA-1",
                    "SHA2-224",
                    "SHA2-256",
                    "SHA2-384",
                    "SHA2-512",
                    "SHA2-512/224",
                    "SHA2-512/256",
                    "SHA3-224",
                    "SHA3-256",
                    "SHA3-384",
                    "SHA3-512"
                };

        public override int[] SeedLength => new[]
        {
            160,
            224,
            256,
            384,
            512,
            224,
            256,
            224,
            256,
            384,
            512
        };

        public override AlgoMode AlgoMode => AlgoMode.DRBG_HMAC_v1_0;
    }
}
