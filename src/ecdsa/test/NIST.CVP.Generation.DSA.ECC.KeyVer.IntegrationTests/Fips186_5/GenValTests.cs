using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.ECDSA.v1_0.KeyVer;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using ParameterValidator = NIST.CVP.Generation.ECDSA.Fips186_5.KeyVer.ParameterValidator;
using RegisterInjections = NIST.CVP.Generation.ECDSA.Fips186_5.KeyVer.RegisterInjections;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "KeyVer";
        public override string Revision { get; set; } = "FIPS186-5";

        public override AlgoMode AlgoMode => AlgoMode.ECDSA_KeyVer_Fips186_5;
        
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            if (testCase.testPassed != null)
            {
                if (testCase.testPassed == true)
                {
                    testCase.testPassed = false;
                }
                else
                {
                    testCase.testPassed = true;
                }
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Curve = new [] { "p-224" }
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                Curve = ParameterValidator.VALID_CURVES
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}