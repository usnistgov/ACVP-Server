using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.BlockCipher_DF;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.BlockCipher_DF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ConditioningComponent";
        public override string Mode { get; } = "BlockCipher_DF";
        public override string Revision { get; set; } = "SP800-90B";

        public override AlgoMode AlgoMode => AlgoMode.ConditioningComponent_BlockCipher_DF_90B;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            var bs = new BitString(testCase.requestedBits.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.requestedBits = bs.ToHex();
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                KeyLen = new[] { 128 },
                OutputLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
                PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(128)),
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
                KeyLen = new[] { 128, 192, 256 },
                OutputLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 512, 8)),
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 65536, 8)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
