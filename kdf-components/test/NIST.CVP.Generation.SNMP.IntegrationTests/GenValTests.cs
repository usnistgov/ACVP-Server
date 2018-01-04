using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using KDFComponent;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SNMP.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "snmp";

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

            if (testCase.sharedKey != null)
            {
                var bs = new BitString(testCase.sharedKey.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sharedKey = bs.ToHex();
            }
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                EngineId = new [] {"12345678912345678900"},
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(128)),
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
                EngineId = new [] {"12345678912345678900", "abcdef0123456789abcdef1234567890"},
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(128)).AddSegment(new ValueDomainSegment(1024)),
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
                EngineId = new [] {"12345678912345678900", "abcdef0123456789abcdef1234567890"},
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(ParameterValidator.PASSWORD_MINIMUM_LENGTH)).AddSegment(new ValueDomainSegment(ParameterValidator.PASSWORD_MAXIMUM_LENGTH)),
                IsSample = false
            };

            return CreateRegistration(folderName, p);
        }
    }
}
