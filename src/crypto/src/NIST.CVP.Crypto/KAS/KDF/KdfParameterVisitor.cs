using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
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
    }
}