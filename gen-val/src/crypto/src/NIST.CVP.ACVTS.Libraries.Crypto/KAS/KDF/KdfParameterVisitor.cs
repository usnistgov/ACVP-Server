using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
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
            var fiPieces = kdfConfiguration.FixedInfoPattern.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList();
            var fiPiecesRemovedDelimiter = fiPieces.Select(fiPiece => fiPiece.Replace("||", "")).ToList();

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
                T = GetNullOrT(kdfConfiguration.FixedInfoPattern),
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
            };

            if (kdfConfiguration.SaltLen > 0)
            {
                param.Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default
                    ? new BitString(kdfConfiguration.SaltLen)
                    : _entropyProvider.GetEntropy(kdfConfiguration.SaltLen);
            }

            return param;
        }

        public IKdfParameter CreateParameter(OneStepNoCounterConfiguration kdfConfiguration)
        {
            var fiPieces = kdfConfiguration.FixedInfoPattern.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList();
            var fiPiecesRemovedDelimiter = fiPieces.Select(fiPiece => fiPiece.Replace("||", "")).ToList();

            var param = new KdfParameterOneStepNoCounter()
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
                T = GetNullOrT(kdfConfiguration.FixedInfoPattern),
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
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
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
            };
        }

        public IKdfParameter CreateParameter(HkdfConfiguration kdfConfiguration)
        {
            return new KdfParameterHkdf
            {
                HmacAlg = kdfConfiguration.HmacAlg,
                L = kdfConfiguration.L,
                FixedInfoPattern = kdfConfiguration.FixedInfoPattern,
                FixedInputEncoding = kdfConfiguration.FixedInfoEncoding,
                Salt = kdfConfiguration.SaltMethod == MacSaltMethod.Default ?
                    new BitString(kdfConfiguration.SaltLen) :
                    _entropyProvider.GetEntropy(kdfConfiguration.SaltLen),

                // If the fixedInfoPattern contains these optional context specific fields, make up a value for them
                Context = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Context), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AlgorithmId = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.AlgorithmId), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                Label = kdfConfiguration.FixedInfoPattern.Contains(nameof(KdfParameterOneStep.Label), StringComparison.OrdinalIgnoreCase) ?
                    _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
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
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
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
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
            };
        }

        public IKdfParameter CreateParameter(Tls10_11Configuration kdfConfiguration)
        {
            return new KdfParameterTls10_11()
            {
                L = kdfConfiguration.L,
                AdditionalInitiatorNonce = kdfConfiguration.ServerGenerateInitiatorAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                AdditionalResponderNonce = kdfConfiguration.ServerGenerateResponderAdditionalNonce ? _entropyProvider.GetEntropy(BitsOfEntropy) : null,
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
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
                EntropyBits = GetEntropyBitsOrNull(kdfConfiguration.FixedInfoPattern),
            };
        }

        private BitString GetNullOrT(string fixedInfoPattern)
        {
            var fiPieces = fixedInfoPattern.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList();
            var fiPiecesRemovedDelimiter = fiPieces.Select(fiPiece => fiPiece.Replace("||", "")).ToList();

            return fiPiecesRemovedDelimiter.Any(a => a == "t") ? _entropyProvider.GetEntropy(BitsOfEntropy) : null;
        }

        private BitString GetEntropyBitsOrNull(string kdfConfigurationFixedInfoPattern)
        {
            const string entropyBits = "entropyBits";

            if (!kdfConfigurationFixedInfoPattern.Contains(entropyBits))
            {
                return null;
            }

            var parts = kdfConfigurationFixedInfoPattern.Split("||");
            var entropyBitsPart = parts.First(w => w.Contains(entropyBits));
            entropyBitsPart = entropyBitsPart
                .Replace("||", "")
                .Replace($"{entropyBits}[", "")
                .Replace("]", "");
            int.TryParse(entropyBitsPart, out var result);

            return _entropyProvider.GetEntropy(result);
        }
    }
}
