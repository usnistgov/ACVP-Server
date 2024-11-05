using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.SigVer.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "SLH-DSA";
    public override string Mode { get; } = "sigVer";
    public override string Revision { get; set; } = "FIPS205";

    public override AlgoMode AlgoMode => AlgoMode.SLH_DSA_SigVer_FIPS205;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();  
    
    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        if (testCase.testPassed != null)
        {
            testCase.testPassed = testCase.testPassed != true;
        }
    }
    
    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var p = new Parameters
        {
            VectorSetId = 53,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHAKE_128s, SlhdsaParameterSet.SLH_DSA_SHA2_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 65536, 8))
                }
            },
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }
    
    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        // Test all the SLH-DSA parameter sets            
        var firstCapabilityParameterSets = EnumHelpers.GetEnumsWithoutDefault<SlhdsaParameterSet>();
        firstCapabilityParameterSets.Remove(SlhdsaParameterSet.SLH_DSA_SHAKE_128f); // remove SLH_DSA_SHAKE_128f as it's already contained in the second capability    
        // Test a representative set of SLH-DSA parameter sets
        //var firstCapabilityParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHA2_192s, SlhdsaParameterSet.SLH_DSA_SHA2_256f, SlhdsaParameterSet.SLH_DSA_SHAKE_192s, SlhdsaParameterSet.SLH_DSA_SHAKE_256f };
        var p = new Parameters
        {
            VectorSetId = 53,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            Capabilities = new []
            {
                new Capability()
                {
                    ParameterSets = firstCapabilityParameterSets.ToArray(),
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 65536, 8))
                },
                new Capability()
                {
                    ParameterSets = new[] { SlhdsaParameterSet.SLH_DSA_SHAKE_128f },
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1024, 4096, 8))
                }
            },
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }
}
