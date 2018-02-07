using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha1 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA-1";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA1";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-224";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-224";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-256";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-256";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_384 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-384";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-384";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-512";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512/224";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-512/224";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha2_512256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA2-512/256";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA2-512/256";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_224 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-224";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA3-224";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_256 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-256";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA3-256";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_384 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-384";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA3-384";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsHmacSha3_512 : GenValHmacBase
    {
        public override string Algorithm => "HMAC-SHA3-512";
        public override string RunnerAlgorithm => "HMAC";
        public override string RunnerMode => "SHA3-512";
    }
}
