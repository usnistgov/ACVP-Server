using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.Hash_DF;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Hash_DF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "ConditioningComponent";
        public override string Mode { get; } = "Hash_DF";
        public override string Revision { get; set; } = "SP800-90B";

        public override AlgoMode AlgoMode => AlgoMode.ConditioningComponent_Hash_DF_90B;

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
                Capabilities = new[]
                {
                    new Capabilities
                    {
                        HashAlg = new [] {ParameterValidator.VALID_HASH_ALG.First()},
                        PayloadLen = new MathDomain().AddSegment(new ValueDomainSegment(256))
                    }
                },
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
                Capabilities = new[]
                {
                    new Capabilities
                    {
                        HashAlg = ParameterValidator.VALID_HASH_ALG,
                        PayloadLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 65536))
                    }
                },
                IsSample = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
