using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.KeyGen.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "SLH-DSA";
    public override string Mode { get; } = "keyGen";
    public override string Revision { get; set; } = "FIPS205";

    public override AlgoMode AlgoMode => AlgoMode.SLH_DSA_KeyGen_FIPS205;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        var rand = new Random800_90();

        var oldValue = new BitString(testCase.sk.ToString());
        var newValue = rand.GetDifferentBitStringOfSameSize(oldValue);
        testCase.sk = newValue.ToHex();
    }
    
    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 53,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            ParameterSets = new [] { SlhdsaParameterSet.SLH_DSA_SHA2_128f },
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 53,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            ParameterSets = ParameterValidator.VALID_PARAMETER_SETS,
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }
}
