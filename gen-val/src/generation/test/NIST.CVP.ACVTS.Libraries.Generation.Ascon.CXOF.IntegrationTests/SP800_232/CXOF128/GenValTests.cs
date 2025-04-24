using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.CXOF128;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.CXOF128.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "Ascon";
    public override string Mode { get; } = "CXOF128";
    public override string Revision { get; set; } = "SP800-232";

    public override AlgoMode AlgoMode => AlgoMode.ASCON_CXOF128_SP800_232;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        if (testCase.digest != null)
        {
            if (testCase.digest.ToString().Length == 0)
            {
                testCase.digest = "AB";
                return;
            }
            var rand = new Random800_90();

            var bs = new BitString(testCase.digest.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.digest = bs.ToHex();
        }
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        MathDomain messageLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain digestLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 65536, 1));
        MathDomain csLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 2048, 1));

        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,
            MessageLength = messageLengths,
            DigestLength = digestLengths,
            CustomizationStringLength = csLengths
        };

        return CreateRegistration(targetFolder, p);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        MathDomain messageLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain digestLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 65536, 1));
        MathDomain csLengths = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 2048, 1));

        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,
            MessageLength = messageLengths,
            DigestLength = digestLengths,
            CustomizationStringLength = csLengths
        };

        return CreateRegistration(targetFolder, p);
    }
}
