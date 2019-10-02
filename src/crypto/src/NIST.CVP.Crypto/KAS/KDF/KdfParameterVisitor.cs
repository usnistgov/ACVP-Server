using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfParameterVisitor : IKdfParameterVisitor
    {
        private readonly IEntropyProvider _entropyProvider;

        public KdfParameterVisitor(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
        }

        public IKdfParameter CreateParameter(OneStepConfiguration kdfConfiguration)
        {
            var param = new KdfParameterOneStep()
            {
                L = kdfConfiguration.L,
                AuxFunction = kdfConfiguration.AuxFunction,
                FixedInfoPattern = kdfConfiguration.FixedInputPattern,
                FixedInputEncoding = kdfConfiguration.FixedInputEncoding,
            };

            if (kdfConfiguration.SaltLen > 0)
            {
                param.Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default
                    ? new BitString(kdfConfiguration.SaltLen)
                    : _entropyProvider.GetEntropy(kdfConfiguration.SaltLen);
            }

            return param;
        }

        public IKdfParameter CreateParameter(TwoStepConfiguration kdfConfiguration)
        {
            return new KdfParameterTwoStep
            {
                L = kdfConfiguration.L,
                FixedInfoPattern = kdfConfiguration.FixedInputPattern,
                FixedInputEncoding = kdfConfiguration.FixedInputEncoding,
                Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default ?
                    new BitString(kdfConfiguration.SaltLen) :
                    _entropyProvider.GetEntropy(kdfConfiguration.SaltLen),
                Iv = kdfConfiguration.IvLen == 0 ?
                    null :
                    _entropyProvider.GetEntropy(kdfConfiguration.IvLen),
                KdfMode = kdfConfiguration.KdfMode,
                MacMode = kdfConfiguration.MacMode,
                CounterLen = kdfConfiguration.CounterLen,
                CounterLocation = kdfConfiguration.CounterLocation
            };
        }
    }
}