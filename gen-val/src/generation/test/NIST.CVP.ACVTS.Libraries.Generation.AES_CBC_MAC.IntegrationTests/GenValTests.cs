using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.CBC_MAC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC_MAC.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ConditioningComponent";
        public override string Mode { get; } = "AES-CBC-MAC";
        public override string Revision { get; set; } = "SP800-90B";

        public override AlgoMode AlgoMode => AlgoMode.ConditioningComponent_CBC_MAC_90B;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            var bs = new BitString(testCase.ct.ToString());
            bs = rand.GetDifferentBitStringOfSameSize(bs);
            testCase.ct = bs.ToHex();
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var p = new Parameters
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                KeyLen = new[] { ParameterValidator.VALID_KEY_LENGTHS.First() },
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
                KeyLen = ParameterValidator.VALID_KEY_LENGTHS,
                PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 65536, 128)),
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
