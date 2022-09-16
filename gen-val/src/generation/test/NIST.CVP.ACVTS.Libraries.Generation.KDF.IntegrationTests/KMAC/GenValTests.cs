using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.IntegrationTests.KMAC
{
    [TestFixture, FastIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "KDF";
        public override string Mode => "KMAC";
        public override string Revision => "Sp800-108r1";

        public override AlgoMode AlgoMode => AlgoMode.KDF_KMAC_Sp800_108r1;
        
        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.derivedKey != null)
            {
                var bs = new BitString(testCase.derivedKey.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.derivedKey = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                IsSample = true,
                ContextLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 1024, 8)),
                DerivedKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 112, 256, 8)),
                KeyDerivationKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 112, 4096, 8)),
                MacMode = new [] { MacModes.KMAC_128 }
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
                IsSample = false,
                ContextLength = new MathDomain().AddSegment(new RangeDomainSegment(null, ParameterValidator.MIN_OTHER_INFO_LENGTH, ParameterValidator.MAX_OTHER_INFO_LENGTH, 8)),
                DerivedKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, ParameterValidator.MIN_KEY_DERIVATION_KEY_LENGTH, ParameterValidator.MAX_KEY_DERIVATION_KEY_LENGTH, 8)),
                KeyDerivationKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, ParameterValidator.MIN_KEY_DERIVATION_KEY_LENGTH, ParameterValidator.MAX_KEY_DERIVATION_KEY_LENGTH, 8)),
                LabelLength = new MathDomain().AddSegment(new RangeDomainSegment(null, ParameterValidator.MIN_OTHER_INFO_LENGTH, ParameterValidator.MAX_OTHER_INFO_LENGTH, 8)),
                MacMode = new [] { MacModes.KMAC_128, MacModes.KMAC_256 }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
