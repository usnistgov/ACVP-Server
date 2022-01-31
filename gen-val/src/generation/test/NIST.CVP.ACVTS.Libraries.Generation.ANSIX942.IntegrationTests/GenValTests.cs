using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.ANSIX942.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ansix9.42";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_ANSIX942_v1_0;

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
                KdfType = new[] { AnsiX942Types.Der },
                Oid = new[] { AnsiX942Oids.TDES },
                HashAlg = new[] { "SHA2-224", "SHA2-256", "SHA2-384" },
                KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                ZzLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                //OtherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                SuppInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(256)),
                IsSample = true // doesn't do anything
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
                KdfType = EnumHelpers.GetEnumsWithoutDefault<AnsiX942Types>().ToArray(),
                Oid = EnumHelpers.GetEnumsWithoutDefault<AnsiX942Oids>().ToArray(),
                HashAlg = ParameterValidator.VALID_HASH_ALG,
                KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                ZzLen = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 64, 4096, 8)),
                OtherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                SuppInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(256)),
                IsSample = false    // doesn't do anything
            };

            return CreateRegistration(folderName, p);
        }
    }
}
