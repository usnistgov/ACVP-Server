using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using IKdfFactory = NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.IKdfFactory;
using KdfResult = NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.KdfResult;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
{
    public class KdfMultiExpansionVisitor : IKdfMultiExpansionVisitor
    {
        private readonly IKdfFactory _kdfTwoStepFactory;
        private readonly IHkdfFactory _hkdfFactory;
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;

        public KdfMultiExpansionVisitor(
            IKdfFactory kdfTwoStepFactory,
            IHkdfFactory hkdfFactory,
            ICmacFactory cmacFactory,
            IHmacFactory hmacFactory
            )
        {
            _kdfTwoStepFactory = kdfTwoStepFactory;
            _hkdfFactory = hkdfFactory;
            _cmacFactory = cmacFactory;
            _hmacFactory = hmacFactory;
        }

        public KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterHkdf param)
        {
            var kdf = _hkdfFactory.GetKdf(ShaAttributes.GetHashFunctionFromEnum(param.HmacAlg));

            List<KdfResult> result = new List<KdfResult>();
            foreach (var iterationParameter in param.IterationParameters)
            {
                result.Add(kdf.DeriveKey(param.Salt, param.Z, iterationParameter.FixedInfo, iterationParameter.L / BitString.BITSINBYTE));
            }

            return new KdfMultiExpansionResult(result);
        }

        public KdfMultiExpansionResult Kdf(KdfMultiExpansionParameterTwoStep param)
        {
            IMac randomnessExtractionMac = null;
            MacModes keyExpansionMacMode = param.MacMode;
            switch (param.MacMode)
            {
                case MacModes.CMAC_AES128:
                    randomnessExtractionMac = _cmacFactory.GetCmacInstance(CmacTypes.AES128);
                    break;
                case MacModes.CMAC_AES192:
                    randomnessExtractionMac = _cmacFactory.GetCmacInstance(CmacTypes.AES192);
                    keyExpansionMacMode = MacModes.CMAC_AES128;
                    break;
                case MacModes.CMAC_AES256:
                    randomnessExtractionMac = _cmacFactory.GetCmacInstance(CmacTypes.AES256);
                    keyExpansionMacMode = MacModes.CMAC_AES128;
                    break;
                case MacModes.HMAC_SHA1:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));
                    break;
                case MacModes.HMAC_SHA224:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224));
                    break;
                case MacModes.HMAC_SHA256:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                    break;
                case MacModes.HMAC_SHA384:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384));
                    break;
                case MacModes.HMAC_SHA512:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));
                    break;
                case MacModes.HMAC_SHA_d512t224:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224));
                    break;
                case MacModes.HMAC_SHA_d512t256:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t256));
                    break;
                case MacModes.HMAC_SHA3_224:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224));
                    break;
                case MacModes.HMAC_SHA3_256:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d256));
                    break;
                case MacModes.HMAC_SHA3_384:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d384));
                    break;
                case MacModes.HMAC_SHA3_512:
                    randomnessExtractionMac =
                        _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d512));
                    break;
                default:
                    throw new ArgumentException($"Invalid {nameof(MacModes)} provided to KdfVisitor.");
            }

            // Randomness extraction (step one)
            var randomnessExtraction = randomnessExtractionMac.Generate(param.Salt, param.Z);

            // Key Expansion (step two)
            var kdf = _kdfTwoStepFactory.GetKdfInstance(
                param.KdfMode,
                keyExpansionMacMode,
                param.CounterLocation,
                param.CounterLen);

            var kdfResults = new List<KdfResult>();
            foreach (var iterationParameter in param.IterationParameters)
            {
                kdfResults.Add(kdf.DeriveKey(randomnessExtraction.Mac, iterationParameter.FixedInfo, iterationParameter.L, param.Iv, 0));
            }

            return new KdfMultiExpansionResult(kdfResults);
        }
    }
}
