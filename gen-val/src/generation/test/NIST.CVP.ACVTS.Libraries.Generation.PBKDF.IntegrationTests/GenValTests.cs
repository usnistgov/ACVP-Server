using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "PBKDF";
        public override string Mode => null;

        public override AlgoMode AlgoMode => AlgoMode.PBKDF_v1_0;

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
                Capabilities = new[]
                {
                    new Capability
                    {
                        HashAlg = new []{"SHA2-256", "SHA3-256"},
                        IterationCount = new MathDomain().AddSegment(new ValueDomainSegment(10)),
                        KeyLength = new MathDomain().AddSegment(new ValueDomainSegment(1024)),
                        PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(32)),
                        SaltLength = new MathDomain().AddSegment(new ValueDomainSegment(128))
                    },
                    new Capability
                    {
                        HashAlg = new []{"SHA-1"},
                        IterationCount = new MathDomain().AddSegment(new ValueDomainSegment(10000)),
                        KeyLength = new MathDomain().AddSegment(new ValueDomainSegment(512)),
                        PasswordLength = new MathDomain().AddSegment(new ValueDomainSegment(16)),
                        SaltLength = new MathDomain().AddSegment(new ValueDomainSegment(256))
                    }
                }
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
                Capabilities = new[]
                {
                    new Capability
                    {
                        //HashAlg = ParameterValidator.VALID_HASH_ALGS,
                        //HashAlg = new[] { "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512" },
                        HashAlg = new[] { "SHA2-224" },
                        IterationCount = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 10000)),
                        KeyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 112, 2048, 8)),
                        PasswordLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 8, 64)),
                        SaltLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 128, 512, 8))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
