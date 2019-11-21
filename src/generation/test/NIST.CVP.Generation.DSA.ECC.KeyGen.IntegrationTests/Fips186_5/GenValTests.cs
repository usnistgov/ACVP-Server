using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using ParameterValidator = NIST.CVP.Generation.ECDSA.Fips186_5.KeyGen.ParameterValidator;
using RegisterInjections = NIST.CVP.Generation.ECDSA.Fips186_5.KeyGen.RegisterInjections;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.IntegrationTests.Fips186_5
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ECDSA";
        public override string Mode { get; } = "KeyGen";
        public override string Revision { get; set; } = "FIPS186-5";
        
        public override AlgoMode AlgoMode => AlgoMode.ECDSA_KeyGen_Fips186_5;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();
            if (testCase.qx != null)
            {
                testCase.qx = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.qx.ToString())).ToHex();
            }

            if (testCase.qy != null)
            {
                testCase.qy = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.qy.ToString())).ToHex();
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
                Curve = new [] { "p-224" },
                SecretGenerationMode = new [] { "testing candidates" }
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
                Curve = ParameterValidator.VALID_CURVES,
                SecretGenerationMode = ParameterValidator.VALID_SECRET_GENERATION_MODES
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}