 using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.AEAD128.IntegrationTests;

[TestFixture, FastIntegrationTest]
public class GenValTests : GenValTestsSingleRunnerBase
{
    public override string Algorithm { get; } = "Ascon";
    public override string Mode { get; } = "AEAD128";
    public override string Revision { get; set; } = "SP800-232";

    public override AlgoMode AlgoMode => AlgoMode.ASCON_AEAD128_SP800_232;

    public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

    protected override void ModifyTestCaseToFail(dynamic testCase)
    {
        if (testCase.tag != null)
        {
            var rand = new Random800_90();

            var bs = new BitString(testCase.tag.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.tag = bs.ToHex();
        }

        if (testCase.pt != null )
        {
            if(testCase.pt.ToString().Length == 0)
            {
                testCase.pt = "AB";
                return;
            }
            var rand = new Random800_90();

            var bs = new BitString(testCase.pt.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.pt = bs.ToHex();
        }
    }

    protected override string GetTestFileFewTestCases(string targetFolder)
    {
        MathDomain plainLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain adLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain truncLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 64, 128, 1));

        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,
            Directions = new[] {BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt},
            PayloadLength = plainLens,
            ADLength = adLens,
            TagLength = truncLens,
            SupportsNonceMasking = new[] { true, false }
        };

        return CreateRegistration(targetFolder, p);
    }

    protected override string GetTestFileLotsOfTestCases(string targetFolder)
    {
        MathDomain plainLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain adLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65536, 1));
        MathDomain truncLens = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 64, 128, 1));

        var p = new Parameters
        {
            VectorSetId = 42,
            Algorithm = Algorithm,
            Mode = Mode,
            Revision = Revision,
            IsSample = true,
            Directions = new[] { BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt },
            PayloadLength = plainLens,
            ADLength = adLens,
            TagLength = truncLens,
            SupportsNonceMasking = new[] { true, false }
        };

        return CreateRegistration(targetFolder, p);
    }
}
