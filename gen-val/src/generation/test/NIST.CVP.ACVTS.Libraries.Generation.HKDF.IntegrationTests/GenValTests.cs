using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.HKDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.HKDF.IntegrationTests
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override AlgoMode AlgoMode => AlgoMode.HKDF_v1_0;
        public override string Algorithm { get; } = "HKDF";
        public override string Mode { get; } = null;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if ((int)testCase.tcId % 2 == 0)
            {
                testCase.derivedKey = rand.GetDifferentBitStringOfSameSize(new BitString(testCase.derivedKey.ToString())).ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new[]
                {
                    new Capability
                    {
                        HmacAlg = new [] {"SHA2-256"},
                        InfoLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 32, 8)),
                        InputKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 32, 8)),
                        SaltLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 32, 8)),
                        KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 32, 8))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var p = new Parameters
            {
                VectorSetId = 42,
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new[]
                {
                    new Capability
                    {
                        HmacAlg = ParameterValidator.VALID_HASH_ALGS,
                        InfoLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 512, 8)),
                        InputKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 4096, 8)),
                        SaltLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 512, 8)),
                        KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 4096, 8))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
