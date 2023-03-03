using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.Shared;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.SigGen.IntegrationTests;

[TestFixture, LongRunningIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override AlgoMode AlgoMode => AlgoMode.LMS_SigGen_v1_0;
    public override string Algorithm => "LMS";
    public override string Mode => "sigGen";
    public override string Revision => "1.0";
    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
    
    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        var rand = new Random800_90();

        var oldValue = new BitString(testCase.signature.ToString());
        var newValue = rand.GetDifferentBitStringOfSameSize(oldValue);
        testCase.signature = newValue.ToHex();
    }

    protected override string GetTestFileFewTestCases(string folderName)
    {
        var p = new Parameters
        {
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,    // Need isSample so that we get responses
            Capabilities = new GeneralCapabilities
            {
                LmsModes = new[] { LmsMode.LMS_SHA256_M24_H5 },
                LmOtsModes = new[] { LmOtsMode.LMOTS_SHA256_N24_W1 }
            }
        };
        
        return CreateRegistration(folderName, p);
    }

    protected override string GetTestFileLotsOfTestCases(string folderName)
    {
        var p = new Parameters
        {
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,    // Need isSample so that we get responses
            Capabilities = new GeneralCapabilities
            {
                LmsModes = new[] { LmsMode.LMS_SHA256_M24_H5, LmsMode.LMS_SHA256_M24_H10 },
                LmOtsModes = new[] { LmOtsMode.LMOTS_SHA256_N24_W1, LmOtsMode.LMOTS_SHA256_N24_W2 }
            }
        };
        
        return CreateRegistration(folderName, p);
    }
}
