using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "BLAKE2b";
    public override string Mode { get; } = null!;
    public override string Revision { get; set; } = "RFC7693";

    public override AlgoMode AlgoMode => AlgoMode.BLAKE2b_RFC7693;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        if (testCase.md == null)
        {
            return;
        }

        var rand = new Random800_90();
        var digest = new BitString(testCase.md.ToString());
        testCase.md = rand.GetDifferentBitStringOfSameSize(digest).ToHex();
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        var parameters = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,
            DigestLength = new MathDomain().AddSegment(new ValueDomainSegment(512)),
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 2048, 8)),
            KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 512, 8))
        };

        return CreateRegistration(targetFolder, parameters);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        var parameters = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = false,
            DigestLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 512, 8)),
            MessageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 8)),
            KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 512, 8))
        };

        return CreateRegistration(targetFolder, parameters);
    }
}