using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.Shared;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SigVer.IntegrationTests.SP800_208;

[TestFixture, LongRunningIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override AlgoMode AlgoMode => AlgoMode.XMSS_SigVer_SP800_208;
    public override string Algorithm => "XMSS";
    public override string Mode => "sigVer";
    public override string Revision => "SP800-208";
    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        if (testCase.testPassed != null)
        {
            testCase.testPassed = !(bool)testCase.testPassed;
        }
    }

    protected override string GetTestFileFewTestCases(string folderName)
    {
        var p = new Parameters
        {
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            Capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_192]
            },
            MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(1024))
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
            Capabilities = new GeneralCapabilities
            {
                XmssModes = [XmssMode.XMSS_SHA2_10_192, XmssMode.XMSS_SHA2_10_256]
            },
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, ParameterValidator.MIN_MESSAGE_LENGTH, ParameterValidator.MAX_MESSAGE_LENGTH, 8))
        };

        return CreateRegistration(folderName, p);
    }
}
