using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls10_11;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTls12;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfParameterVisitor : IKdfParameterVisitor
    {
        private const int BitsOfEntropy = 128;

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
                FixedInfoPattern = kdfConfiguration.FixedInfoPattern,
                FixedInputEncoding = kdfConfiguration.FixedInfoEncoding,
                // If the fixedInfoPattern contains these optional context specific fields, make up a value for them
                Context = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Context), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AlgorithmId = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.AlgorithmId), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                Label = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Label), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,

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
                FixedInfoPattern = kdfConfiguration.FixedInfoPattern,
                FixedInputEncoding = kdfConfiguration.FixedInfoEncoding,
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
                // If the fixedInfoPattern contains these optional context specific fields, make up a value for them
                Context = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Context), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AlgorithmId = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.AlgorithmId), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                Label = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Label), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
            };
        }

        public IKdfParameter CreateParameter(IkeV1Configuration kdfConfiguration)
        {
            return new KdfParameterIkeV1()
            {
                L = kdfConfiguration.L,
                HashFunction = kdfConfiguration.HashFunction,
                AdditionalInitiatorNonce = kdfConfiguration.ServerGenerateInitiatorAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AdditionalResponderNonce = kdfConfiguration.ServerGenerateResponderAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
            };
        }

        public IKdfParameter CreateParameter(IkeV2Configuration kdfConfiguration)
        {
            return new KdfParameterIkeV2()
            {
                L = kdfConfiguration.L,
                HashFunction = kdfConfiguration.HashFunction,
                AdditionalInitiatorNonce = kdfConfiguration.ServerGenerateInitiatorAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AdditionalResponderNonce = kdfConfiguration.ServerGenerateResponderAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
            };
        }

        public IKdfParameter CreateParameter(Tls10_11Configuration kdfConfiguration)
        {
            return new KdfParameterTls10_11()
            {
                L = kdfConfiguration.L,
                AdditionalInitiatorNonce = kdfConfiguration.ServerGenerateInitiatorAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AdditionalResponderNonce = kdfConfiguration.ServerGenerateResponderAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
            };
        }

        public IKdfParameter CreateParameter(Tls12Configuration kdfConfiguration)
        {
            return new KdfParameterTls12()
            {
                L = kdfConfiguration.L,
                HashFunction = kdfConfiguration.HashFunction,
                AdditionalInitiatorNonce = kdfConfiguration.ServerGenerateInitiatorAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AdditionalResponderNonce = kdfConfiguration.ServerGenerateResponderAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
            };
        }
    }
}