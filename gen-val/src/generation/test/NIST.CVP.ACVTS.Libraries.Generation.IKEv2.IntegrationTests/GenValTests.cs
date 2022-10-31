using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.ACVTS.Libraries.Generation.Tests;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.IKEv2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "ikev2";

        public override AlgoMode AlgoMode => AlgoMode.KDFComponents_IKEv2_v1_0;

        public override IRegisterInjections RegistrationsGenVal => new RegisterInjections();


        protected override void ModifyTestCaseToFail(dynamic testCase)
        {
            var rand = new Random800_90();

            if (testCase.sKeySeed != null)
            {
                var bs = new BitString(testCase.sKeySeed.ToString());
                bs = rand.GetDifferentBitStringOfSameSize(bs);

                testCase.sKeySeed = bs.ToHex();
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
                Capabilities = new[]
                {
                    new Capabilities
                    {
                        HashAlg = new [] {"SHA-1"},
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8))
                    },
                    new Capabilities
                    {
                        HashAlg = new [] {"SHA2-224"},
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DerivedKeyingMaterialChildLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 512, 1024, 8))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var rand = new Random800_90();
            var firstHashes = new[] { "SHA-1", "SHA2-224", "SHA2-256" };
            var lastHashes = new[] { "SHA2-384", "SHA2-512" };
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new[]
                {
                    new Capabilities
                    {
                        HashAlg = firstHashes,
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 384, ParameterValidator.MAX_DKM))
                    },
                    new Capabilities
                    {
                        HashAlg = lastHashes,
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_NONCE, ParameterValidator.MAX_NONCE)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, ParameterValidator.MIN_DH, ParameterValidator.MAX_DH)),
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 1024, ParameterValidator.MAX_DKM)),
                        DerivedKeyingMaterialChildLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 768, ParameterValidator.MAX_DKM))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
