using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX963;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.ANSIX963.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ansix9.63";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_ANSIX963_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.keyData != null)
            {
                var bs = new BitString(testCase.keyData.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.keyData = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                HashAlg = new[] { "SHA2-224", "SHA2-256", "SHA2-384" },
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new[] { 224, 283 },
                IsSample = true
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
                HashAlg = ParameterValidator.VALID_HASH_ALGS,
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new[] { 224, 521 },
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
