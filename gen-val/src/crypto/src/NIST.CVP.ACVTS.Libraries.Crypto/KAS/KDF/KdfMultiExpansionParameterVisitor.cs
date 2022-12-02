using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
{
    public class KdfMultiExpansionParameterVisitor : IKdfMultiExpansionParameterVisitor
    {
        private const int BitsOfEntropy = 128;

        private readonly IEntropyProvider _entropyProvider;
        private readonly IRandom800_90 _random;

        public KdfMultiExpansionParameterVisitor(IEntropyProvider entropyProvider, IRandom800_90 random)
        {
            _entropyProvider = entropyProvider;
            _random = random;
        }

        public IKdfMultiExpansionParameter CreateParameter(TwoStepMultiExpansionConfiguration kdfConfiguration)
        {
            var iterationCount = _random.GetRandomInt(2, 5);
            var iterationParameters = new List<KdfMultiExpansionIterationParameter>();

            for (var i = 0; i < iterationCount; i++)
            {
                iterationParameters.Add(new KdfMultiExpansionIterationParameter(kdfConfiguration.L, _entropyProvider.GetEntropy(BitsOfEntropy)));
            }

            return new KdfMultiExpansionParameterTwoStep()
            {
                Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default ?
                    new BitString(kdfConfiguration.SaltLen) :
                    _entropyProvider.GetEntropy(kdfConfiguration.SaltLen),
                Iv = kdfConfiguration.IvLen == 0 ?
                    null :
                    _entropyProvider.GetEntropy(kdfConfiguration.IvLen),
                KdfMode = kdfConfiguration.KdfMode,
                MacMode = kdfConfiguration.MacMode,
                CounterLen = kdfConfiguration.CounterLen,
                CounterLocation = kdfConfiguration.CounterLocation,
                Z = _entropyProvider.GetEntropy(BitsOfEntropy),
                IterationParameters = iterationParameters
            };
        }

        public IKdfMultiExpansionParameter CreateParameter(HkdfMultiExpansionConfiguration kdfConfiguration)
        {
            var iterationCount = _random.GetRandomInt(2, 5);
            var iterationParameters = new List<KdfMultiExpansionIterationParameter>();

            for (var i = 0; i < iterationCount; i++)
            {
                iterationParameters.Add(new KdfMultiExpansionIterationParameter(kdfConfiguration.L, _entropyProvider.GetEntropy(BitsOfEntropy)));
            }

            return new KdfMultiExpansionParameterHkdf()
            {
                HmacAlg = kdfConfiguration.HmacAlg,
                Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default ?
                    new BitString(kdfConfiguration.SaltLen) :
                    _entropyProvider.GetEntropy(kdfConfiguration.SaltLen),
                Z = _entropyProvider.GetEntropy(BitsOfEntropy),
                IterationParameters = iterationParameters
            };
        }
    }
}
