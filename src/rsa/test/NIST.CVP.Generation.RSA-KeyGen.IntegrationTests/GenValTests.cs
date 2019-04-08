using NIST.CVP.Common;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm { get; } = "RSA";
        public override string Mode { get; } = "KeyGen";

        public override AlgoMode AlgoMode => AlgoMode.RSA_KeyGen_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.testPassed != null)
            {
                testCase.testPassed = !(bool) testCase.testPassed;
            }

            // If TC has a cipherText, change it
            if (testCase.p != null)
            {
                BitString bs = new BitString(testCase.p.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.p = bs.ToHex();
            }

            // If TC has a plainText, change it
            if (testCase.q != null)
            {
                BitString bs = new BitString(testCase.q.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                // Can't get something "different" of empty bitstring of the same length
                if (bs == null)
                {
                    bs = new BitString("01");
                }

                testCase.q = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            var caps = new Capability[1];
            caps[0] = new Capability
            {
                Modulo = 2048,
                HashAlgs = new[] { "sha2-224" },
                PrimeTests = new[] { "tblc2" }
            };

            var algSpecs = new AlgSpec[4];
            algSpecs[0] = new AlgSpec
            {
                RandPQ = "b.3.2",
                Capabilities = caps
            };

            algSpecs[1] = new AlgSpec
            {
                RandPQ = "b.3.4",
                Capabilities = caps
            };

            algSpecs[2] = new AlgSpec
            {
                RandPQ = "b.3.5",
                Capabilities = caps
            };

            algSpecs[3] = new AlgSpec
            {
                RandPQ = "b.3.6",
                Capabilities = caps
            };

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                InfoGeneratedByServer = false,
                IsSample = true,
                PubExpMode = "fixed",
                FixedPubExp = "010001",
                KeyFormat = "crt",
                AlgSpecs = algSpecs
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            var caps = new Capability[ParameterValidator.VALID_MODULI.Length];
            for (var i = 0; i < caps.Length; i++)
            {
                caps[i] = new Capability
                {
                    Modulo = ParameterValidator.VALID_MODULI[i],
                    HashAlgs = new string[] { "sha2-224" },
                    PrimeTests = ParameterValidator.VALID_PRIME_TESTS
                };
            }

            var algSpecs = new AlgSpec[ParameterValidator.VALID_KEY_GEN_MODES.Length];
            for (var i = 0; i < algSpecs.Length; i++)
            {
                algSpecs[i] = new AlgSpec
                {
                    RandPQ = ParameterValidator.VALID_KEY_GEN_MODES[i],
                    Capabilities = caps
                };
            }

            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                InfoGeneratedByServer = true,
                IsSample = true,
                PubExpMode = "random",
                AlgSpecs = algSpecs,
                KeyFormat = "crt"
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}
