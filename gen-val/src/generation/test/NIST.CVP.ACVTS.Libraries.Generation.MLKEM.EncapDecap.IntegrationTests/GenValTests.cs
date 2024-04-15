using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.MLKEM.EncapDecap.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "ML-KEM";
    public override string Mode { get; } = "encapDecap";
    public override string Revision { get; set; } = "FIPS203";
    
    public override AlgoMode AlgoMode => AlgoMode.ML_KEM_EncapDecap_FIPS203;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        var rand = new Random800_90();

        if (testCase.k != null)
        {
            var bs = new BitString(testCase.k.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.k = bs.ToHex();
        }
        
        if (testCase.c != null)
        {
            var bs = new BitString(testCase.c.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.c = bs.ToHex();    
        }
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            ParameterSets = new [] { KyberParameterSet.ML_KEM_512 },
            Functions = new [] { KyberFunction.Encapsulation, KyberFunction.Decapsulation },
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
            Functions = new [] { KyberFunction.Encapsulation, KyberFunction.Decapsulation },
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }
}
