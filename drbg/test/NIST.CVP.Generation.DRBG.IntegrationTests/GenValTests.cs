using System.Collections.Generic;
using System.IO;
using Autofac;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using DRBG;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
{
    #region AES
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes128 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-128";
        public override int DataLength => 128;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes192 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-192";
        public override int DataLength => 192;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes256 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-256";
        public override int DataLength => 256;
    }
    #endregion AES

    #region TDES
    [TestFixture, FastIntegrationTest]
    public class GenValTestsTdes : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "TDES";
        public override int DataLength => 112;
    }
    #endregion TDES

    #region Hash
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha1 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA-1";
        public override int DataLength => 160;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha224 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-224";
        public override int DataLength => 224;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha256 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-256";
        public override int DataLength => 256;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha384 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-384";
        public override int DataLength => 384;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha512 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-512";
        public override int DataLength => 512;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha512t224 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-512/224";
        public override int DataLength => 224;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHashSha512t256 : GenValTestsHashBase
    {
        public override string Algorithm => "hashDRBG";
        public override string Mode => "SHA2-512/256";
        public override int DataLength => 256;
    }
    #endregion Hash

    #region HMAC
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha1 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA-1";
        public override int DataLength => 160;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha224 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-224";
        public override int DataLength => 224;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha256 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-256";
        public override int DataLength => 256;
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha384 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-384";
        public override int DataLength => 384;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha512 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-512";
        public override int DataLength => 512;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha512t224 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-512/224";
        public override int DataLength => 224;
    }

    
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHMACSha512t256 : GenValTestsHashBase
    {
        public override string Algorithm => "hmacDRBG";
        public override string Mode => "SHA2-512/256";
        public override int DataLength => 256;
    }
    #endregion HMAC
}
