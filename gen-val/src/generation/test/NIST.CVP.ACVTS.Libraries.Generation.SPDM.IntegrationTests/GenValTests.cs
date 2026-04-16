using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM.IntegrationTests;

[TestFixture, LongRunningIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "KDF";
    public override string Mode { get; } = "SPDM";
    public override string Revision => "1.0";

    public override AlgoMode AlgoMode => AlgoMode.KDF_SPDM_v1_0;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        // If TC has a result, change it
        var rand = new Random800_90();
        testCase.exportMasterSecret = rand.GetDifferentBitStringOfSameSize(new BitString((string) testCase.exportMasterSecret)).ToHex();
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var p = new Parameters()
        {
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            HashAlgs = [HashFunctions.Sha3_d256],
            KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
            SPDMVersion = [SPDMVersions.SPDM13],
            UsesPSK = [true],
            THLen = new MathDomain().AddSegment(new ValueDomainSegment(256)),
        };
            
        return CreateRegistration(targetFolder, p);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        var p = new Parameters()
        {
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            HashAlgs = ParameterValidator.VALID_HASH_FUNCTIONS,
            KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 4096, 8)),
            SPDMVersion = ParameterValidator.VALID_VERSIONS,
            UsesPSK = [true, false],
            THLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 512, 128))
        };

        return CreateRegistration(targetFolder, p);
    }
}
