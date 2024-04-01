using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.KeyGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.MLDSA.KeyGen.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "ML-DSA";
    public override string Mode { get; } = "keyGen";
    public override string Revision { get; set; } = "FIPS204";
    
    public override AlgoMode AlgoMode => AlgoMode.ML_DSA_KeyGen_FIPS204;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        var rand = new Random800_90();

        var bs = new BitString(testCase.pk.ToString());
        bs = rand.GetDifferentBitStringOfSameSize(bs);
        testCase.pk = bs.ToHex();
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            ParameterSets = new [] { DilithiumParameterSet.ML_DSA_44 },
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            ParameterSets = ParameterValidator.VALID_PARAMETER_SETS,
            IsSample = false
        };

        return CreateRegistration(targetFolder, p);
    }
}
