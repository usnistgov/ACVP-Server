using NIST.CVP.Generation.Core.Tests;
using System;
using Autofac;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using RSA_SP_Component;

namespace NIST.CVP.Generation.RSA_SPComponent.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsBase
    {
        public override string Algorithm => "RSA";
        public override string Mode => "SignaturePrimitive";

        public override Executable Generator => Program.Main;
        public override Executable Validator => RSA_SP_Component_Val.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
            RSA_SP_Component_Val.AutofacConfig.OverrideRegistrations = null;
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            return GetTestFileMinimalTestCases(folderName);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = false,
                KeyFormat = "standard"
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
                KeyFormat = "crt"
            };

            return CreateRegistration(folderName, p);
        }

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            // If TC has a result, change it
            if (testCase.isSuccess != null)
            {
                testCase.isSuccess = !((bool)testCase.isSuccess);
            }
        }

        protected override void OverrideRegistrationGenFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureParameterParser<Parameters>>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeException()
        {
            RSA_SP_Component_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            RSA_SP_Component_Val.AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }
    }
}
