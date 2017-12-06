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
    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes128 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-128";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes192 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-192";
    }

    [TestFixture, FastIntegrationTest]
    public class GenValTestsAes256 : GenValTestsDrbgBase
    {
        public override string Algorithm => "ctrDRBG";
        public override string Mode => "AES-256";
    }
}
