using NIST.CVP.Generation.Core.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using Autofac;
using KDFComponent;
using NIST.CVP.Math;
using NUnit.Framework.Internal.Builders;

namespace NIST.CVP.Generation.SSH.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ssh";

        public override Executable Generator => Program.Main;
        public override Executable Validator => KDFComponent_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AdditionalParameters = new[] {Mode};
            KDFComponent.AutofacConfig.OverrideRegistrations = null;
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            KDFComponent.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            KDFComponent_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.initialIvClient != null)
            {
                var bs = new BitString(testCase.initialIvClient.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.initialIvClient = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Cipher = new[] {"tdes"},
                HashAlg = new[] {"sha-1"},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Cipher = new[] {"aes-128"},
                HashAlg = new[] {"sha-1", "sha2-256", "sha2-512"},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Cipher = ParameterValidator.VALID_CIPHERS,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
