using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;
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
            SignatureInterfaces = new [] { SignatureInterface.Internal },
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
        var p = new Parameters
        {
            VectorSetId = 53,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            SignatureInterfaces = [SignatureInterface.External, SignatureInterface.Internal],
            PreHash = [PreHash.Pure, PreHash.PreHash],
            Capabilities =
            [
                new Capability
                {
                    ParameterSets = ParameterValidator.FastSigningParameterSets,
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, PqcParameterValidator.MinMsgLen, PqcParameterValidator.MaxMsgLen, 8)),
                    HashAlgs = PqcParameterValidator.ValidHashAlgs,
                    ContextLength = new MathDomain().AddSegment(new RangeDomainSegment(null, PqcParameterValidator.MinContextLen, PqcParameterValidator.MaxContextLen, 8))
                },
                new Capability
                {
                    ParameterSets = ParameterValidator.SmallSignatureParameterSets,
                    MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, PqcParameterValidator.MinMsgLen, PqcParameterValidator.MaxMsgLen, 8)),
                    HashAlgs = PqcParameterValidator.ValidHashAlgs,
                    ContextLength = new MathDomain().AddSegment(new RangeDomainSegment(null, PqcParameterValidator.MinContextLen, PqcParameterValidator.MaxContextLen, 8))
                }
            ],
            IsSample = true
        };

        return CreateRegistration(targetFolder, p);
    }
}
