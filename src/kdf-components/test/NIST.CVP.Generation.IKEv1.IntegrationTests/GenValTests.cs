using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Common;
using NUnit.Framework;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.IKEv1.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "IKEv1";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_IKEv1_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();
		public override IRegisterInjections RegistrationsCrypto => new Crypto.RegisterInjections();

        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.sKeyId != null)
            {
                var bs = new BitString(testCase.sKeyId.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sKeyId = bs.ToHex();
            }
        }

        protected override string GetTestFileFewTestCases(string folderName)
        {
            var rand = new Random800_90();
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new []
                {
                    new Capability
                    {
                        AuthenticationMethod = "dsa",
                        HashAlg = new [] {"sha-1", "sha2-224", "sha2-512"},
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 64, 256, 8)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 64, 256, 8)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 224, 512, 8)),
                    },
                    new Capability
                    {
                        AuthenticationMethod = "psk",
                        HashAlg = new [] {"sha-1", "sha2-384"},
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 64, 256)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 64, 256)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 224, 512)),
                        PreSharedKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 128, 512))
                    },
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var rand = new Random800_90();
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new []
                {
                    new Capability
                    {
                        AuthenticationMethod = "dsa",
                        HashAlg = ParameterValidator.VALID_HASH_ALGS,
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                    },
                    new Capability
                    {
                        AuthenticationMethod = "psk",
                        HashAlg = ParameterValidator.VALID_HASH_ALGS,
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                        PreSharedKeyLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_PRESHARED_KEY, ParameterValidator.MAX_PRESHARED_KEY))
                    },
                    new Capability
                    {
                        AuthenticationMethod = "pke",
                        HashAlg = ParameterValidator.VALID_HASH_ALGS,
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
