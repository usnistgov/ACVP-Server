using NIST.CVP.Crypto.Common;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
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

        public override AlgoMode AlgoMode => AlgoMode.DRBG_CTR;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHash : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string[] Modes => new[]
        {
            //"SHA-1",
            "SHA2-224",
            "SHA2-256",
            //"SHA2-384",
            //"SHA2-512",
            "SHA2-512/224",
            "SHA2-512/256"
        };

        public override int[] SeedLength => new[]
        {
            //160,
            224,
            256,
            //384,
            //512,
            224,
            256
        };

        public override AlgoMode AlgoMode => AlgoMode.DRBG_Hash;
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
                    "SHA2-512/256"
                };

        public override int[] SeedLength => new[]
        {
            160,
            224,
            256,
            384,
            512,
            224,
            256
        };

        public override AlgoMode AlgoMode => AlgoMode.DRBG_HMAC;
    }
}
