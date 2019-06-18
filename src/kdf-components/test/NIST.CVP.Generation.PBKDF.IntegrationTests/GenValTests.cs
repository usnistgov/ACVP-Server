using System;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.KDF_Components.v1_0.PBKDF;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.PBKDF.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "PBKDF";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_PBKDF_v1_0;

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
                HashAlg = new []{"SHA2-224", "SHA3-256"},
                IterationCount = new MathDomain().AddSegment(new ValueDomainSegment(10)),
                KeyLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(32)),
                SaltLength = new MathDomain().AddSegment(new ValueDomainSegment(128))
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
                HashAlg = new []{"SHA2-224", "SHA3-256", "SHA-1", "SHA2-512", "SHA3-512"},
                IterationCount = new MathDomain().AddSegment(new RangeDomainSegment(null, 10, 10000)),
                KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 112, 256)),
                PasswordLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 16)),
                SaltLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 256))
            };

            return CreateRegistration(folderName, p);
        }
    }
}