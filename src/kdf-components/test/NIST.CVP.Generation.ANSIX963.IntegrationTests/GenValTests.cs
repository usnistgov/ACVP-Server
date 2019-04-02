using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ANSIX963.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ansix9.63";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_ANSIX963_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

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
                HashAlg = new [] {"sha2-224", "sha2-256", "sha2-384"},
                KeyDataLength = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                SharedInfoLength = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                FieldSize = new [] {224, 283},
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
                FieldSize = new [] {224, 521},
                IsSample = true
            };

            return CreateRegistration(folderName, p);
        }
    }
}
