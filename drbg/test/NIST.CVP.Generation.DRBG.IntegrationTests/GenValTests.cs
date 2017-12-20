using System.Collections.Generic;
using System.IO;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using DRBG;

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
    }
}
