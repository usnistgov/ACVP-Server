using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using Autofac;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NUnit.Framework;
using NIST.CVP.Generation.Core.Tests;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Common;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv2;

namespace NIST.CVP.Generation.IKEv2.IntegrationTests
{
    [TestFixture, LongRunningIntegrationTest]
    public class GenValTests : GenValTestsSingleRunnerBase
    {
        public override string Algorithm => "kdf-components";
        public override string Mode => "IKEv2";

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
                Capabilities = new []
                {
                    new Capabilities
                    {
                        HashAlg = new [] {"sha-1", "sha2-224"},
                        InitiatorNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        ResponderNonceLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DiffieHellmanSharedSecretLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8)),
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 256, 1024, 8))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }

        protected override string GetTestFileLotsOfTestCases(string folderName)
        {
            var rand = new Random800_90();
            var firstHashes = new[] {"sha-1", "sha2-224", "sha2-256"};
            var lastHashes = new[] {"sha2-384", "sha2-512"};
            var p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                Revision = Revision,
                Capabilities = new []
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
                        DerivedKeyingMaterialLength = new MathDomain().AddSegment(new RangeDomainSegment(rand, 1024, ParameterValidator.MAX_DKM))
                    }
                }
            };

            return CreateRegistration(folderName, p);
        }
    }
}
