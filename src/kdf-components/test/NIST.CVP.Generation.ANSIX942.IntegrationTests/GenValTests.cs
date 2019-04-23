using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ANSIX942.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ansix9.42";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_ANSIX942_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.keyData != null)
            {
                var bs = new BitString(testCase.derivedKey.ToString());
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
                KdfType = ParameterValidator.VALID_MODES,
                HashAlg = new [] {"sha2-224", "sha2-256", "sha2-384"},
                KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                ZzLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                OtherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
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
                KdfType = ParameterValidator.VALID_MODES,
                HashAlg = ParameterValidator.VALID_HASH_ALG,
                KeyLen = new MathDomain().AddSegment(new ValueDomainSegment(256)).AddSegment(new ValueDomainSegment(1024)),
                ZzLen = new MathDomain().AddSegment(new ValueDomainSegment(8)).AddSegment(new ValueDomainSegment(1024)),
                OtherInfoLen = new MathDomain().AddSegment(new ValueDomainSegment(0)).AddSegment(new ValueDomainSegment(1024)),
                IsSample = false    // doesn't do anything
            };

            return CreateRegistration(folderName, p);
        }
    }
}
