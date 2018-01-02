using NIST.CVP.Generation.Core.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using KDFComponent;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TLS.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "tls";

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

            if (testCase.masterSecret != null)
            {
                var bs = new BitString(testCase.masterSecret.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.masterSecret = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                TlsVersion = new[] {"v1.0/1.1"},
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
                TlsVersion = new[] {"v1.2"},
                HashAlg = new [] {"sha2-256", "sha2-512"},
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
                TlsVersion = ParameterValidator.VALID_TLS_VERSIONS,
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
