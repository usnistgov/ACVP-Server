using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV1;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfIkeV2;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfTwoStep;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;
using System;
using IKdfFactory = NIST.CVP.Crypto.Common.KDF.IKdfFactory;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfVisitor : IKdfVisitor
    {
        private readonly IKdfOneStepFactory _kdfOneStepFactory;
        private readonly IKdfFactory _kdfTwoStepFactory;
        private readonly IHmacFactory _hmacFactory;
        private readonly ICmacFactory _cmacFactory;
        private readonly IIkeV1Factory _ikeV1Factory;
        private readonly IIkeV2Factory _ikeV2Factory;

        public KdfVisitor(IKdfOneStepFactory kdfOneStepFactory,
            IKdfFactory kdfTwoStepFactory,
            IHmacFactory hmacFactory,
            ICmacFactory cmacFactory,
            IIkeV1Factory ikeV1Factory,
            IIkeV2Factory ikeV2Factory)
        {
            _kdfOneStepFactory = kdfOneStepFactory;
            _kdfTwoStepFactory = kdfTwoStepFactory;
            _hmacFactory = hmacFactory;
            _cmacFactory = cmacFactory;
            _ikeV1Factory = ikeV1Factory;
            _ikeV2Factory = ikeV2Factory;
        }

        public KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo)
        {
            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction);

            return kdf.DeriveKey(param.Z, param.L, fixedInfo, param.Salt);
        }

        public KdfResult Kdf(KdfParameterTwoStep param, BitString fixedInfo)
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

            return new KdfResult(kdf.DeriveKey(randomnessExtraction.Mac, fixedInfo, param.L, param.Iv, 0).DerivedKey);
        }

        public KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null)
        {
            var hashFunction = ShaAttributes.GetHashFunctionFromEnum(param.HashFunction);

            var kdf = _ikeV1Factory.GetIkeV1Instance(AuthenticationMethods.Dsa, hashFunction);

            // TODO THIS NEEDS CONFIRMATION FROM CT GROUP
            var result = kdf.GenerateIke(param.InitiatorEphemeralData, param.ResponderEphemeralData, param.Z,
                param.AdditionalInitiatorNonce, param.AdditionalResponderNonce, null);

            // TODO THIS NEEDS CONFIRMATION FROM CT GROUP
            var dkm = new BitString(0)
                .ConcatenateBits(result.SKeyIdD)
                .ConcatenateBits(result.SKeyIdA)
                .ConcatenateBits(result.SKeyIdE);

            return new KdfResult(dkm.GetMostSignificantBits(param.L));
        }

        public KdfResult Kdf(KdfParameterIkeV2 param, BitString fixedInfo = null)
        {
            var hashFunction = ShaAttributes.GetHashFunctionFromEnum(param.HashFunction);

            var kdf = _ikeV2Factory.GetInstance(hashFunction);

            // TODO THIS NEEDS CONFIRMATION FROM CT GROUP
            var result = kdf.GenerateDkmIke(
                param.InitiatorEphemeralData,
                param.ResponderEphemeralData,
                param.Z,
                param.AdditionalInitiatorNonce,
                param.AdditionalResponderNonce,
                param.L);

            return new KdfResult(result);
        }
    }
}