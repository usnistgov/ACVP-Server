using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_DPComponent.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "RSA";
        public override string Mode => "DecryptionPrimitive";

        public override Executable Generator => GenValApp.Program.Main;
        public override Executable Validator => GenValApp.Program.Main;

        [SetUp]
        public override void SetUp()
        {
            AutofacConfig.OverrideRegistrations = null;
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            return GetTestFileMinimalTestCases(folderName);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            return GetTestFileMinimalTestCases(folderName);
        }

        protected override string GetTestFileMinimalTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                IsSample = true,
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

            var rand = new Random800_90();
            // If TC has a pt, change it
            if (testCase.plainText != null)
            {
                var bs = new BitString(testCase.plainText.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.plainText = bs.ToHex();
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
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeExceptionDynamicParser>().AsImplementedInterfaces();
            };
        }

        protected override void OverrideRegistrationValFakeFailure()
        {
            AutofacConfig.OverrideRegistrations = builder =>
            {
                builder.RegisterType<FakeFailureDynamicParser>().AsImplementedInterfaces();
            };
        }
    }
}
