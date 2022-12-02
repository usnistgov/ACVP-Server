using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfHkdf;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfIkeV2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls10_11;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTls12;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfTwoStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using IKdfFactory = NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.IKdfFactory;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
{
    public class KdfVisitor : IKdfVisitor
    {
        private readonly IKdfOneStepFactory _kdfOneStepFactory;
        private readonly IKdfFactory _kdfTwoStepFactory;
        private readonly IHmacFactory _hmacFactory;
        private readonly ICmacFactory _cmacFactory;
        private readonly IIkeV1Factory _ikeV1Factory;
        private readonly IIkeV2Factory _ikeV2Factory;
        private readonly ITlsKdfFactory _tlsFactory;
        private readonly IHkdfFactory _hkdfFactory;

        public KdfVisitor(IKdfOneStepFactory kdfOneStepFactory,
            IKdfFactory kdfTwoStepFactory,
            IHmacFactory hmacFactory,
            ICmacFactory cmacFactory,
            IIkeV1Factory ikeV1Factory,
            IIkeV2Factory ikeV2Factory,
            ITlsKdfFactory tlsFactory,
            IHkdfFactory hkdfFactory)
        {
            _kdfOneStepFactory = kdfOneStepFactory;
            _kdfTwoStepFactory = kdfTwoStepFactory;
            _hmacFactory = hmacFactory;
            _cmacFactory = cmacFactory;
            _ikeV1Factory = ikeV1Factory;
            _ikeV2Factory = ikeV2Factory;
            _tlsFactory = tlsFactory;
            _hkdfFactory = hkdfFactory;
        }

        public KdfResult Kdf(KdfParameterOneStep param, BitString fixedInfo)
        {
            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction, true);

            return kdf.DeriveKey(param.Z, param.L, fixedInfo, param.Salt);
        }

        public KdfResult Kdf(KdfParameterOneStepNoCounter param, BitString fixedInfo)
        {
            var kdf = _kdfOneStepFactory.GetInstance(param.AuxFunction, false);

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

            BitString sharedSecret;
            
            // The IUT supports/uses a hybrid shared secret
            if (param.T != default)
            {
                // Create the hybrid shared secret by concatenating the shared secret (Z) and auxiliary shared secret (T)
                sharedSecret = param.Z.ConcatenateBits(param.T);
            }
            else // The IUT does not support/use a hybrid shared secret
            {
                sharedSecret = param.Z;
            }
            
            // Randomness extraction (step one)
            var randomnessExtraction = randomnessExtractionMac.Generate(param.Salt, sharedSecret);

            // Key Expansion (step two)
            var kdf = _kdfTwoStepFactory.GetKdfInstance(
                param.KdfMode,
                keyExpansionMacMode,
                param.CounterLocation,
                param.CounterLen);

            return new KdfResult(kdf.DeriveKey(randomnessExtraction.Mac, fixedInfo, param.L, param.Iv, 0).DerivedKey);
        }

        public KdfResult Kdf(KdfParameterHkdf param, BitString fixedInfo)
        {
            var kdf = _hkdfFactory.GetKdf(ShaAttributes.GetHashFunctionFromEnum(param.HmacAlg));
            BitString sharedSecret;
            
            // The IUT supports/uses a hybrid shared secret
            if (param.T != default)
            {
                // Create the hybrid shared secret by concatenating the shared secret (Z) and auxiliary shared secret (T)
                sharedSecret = param.Z.ConcatenateBits(param.T);
            }
            else // The IUT does not support/use a hybrid shared secret
            {
                sharedSecret = param.Z;
            }
            
            var result = kdf.DeriveKey(param.Salt, sharedSecret, fixedInfo, param.L / BitString.BITSINBYTE);

            if (result.Success)
            {
                return new KdfResult(result.DerivedKey);
            }

            return new KdfResult(result.ErrorMessage);
        }

        public KdfResult Kdf(KdfParameterIkeV1 param, BitString fixedInfo = null)
        {
            var hashFunction = ShaAttributes.GetHashFunctionFromEnum(param.HashFunction);

            var kdf = _ikeV1Factory.GetIkeV1Instance(AuthenticationMethods.Dsa, hashFunction);

            var result = kdf.GenerateIke(param.InitiatorEphemeralData, param.ResponderEphemeralData, param.Z,
                param.AdditionalInitiatorNonce, param.AdditionalResponderNonce, null);

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

            var result = kdf.GenerateDkmIke(
                param.InitiatorEphemeralData,
                param.ResponderEphemeralData,
                param.Z,
                param.AdditionalInitiatorNonce,
                param.AdditionalResponderNonce,
                param.L);

            return new KdfResult(result);
        }

        public KdfResult Kdf(KdfParameterTls10_11 param, BitString fixedInfo = null)
        {
            var hashFunction = ShaAttributes.GetHashFunctionFromEnum(param.HashFunction);

            var kdf = _tlsFactory.GetTlsKdfInstance(TlsModes.v10v11, hashFunction);

            var result = kdf.DeriveKey(
                param.Z,
                param.InitiatorEphemeralData,
                param.ResponderEphemeralData,
                param.AdditionalInitiatorNonce,
                param.AdditionalResponderNonce,
                param.L);

            return new KdfResult(result.DerivedKey);
        }

        public KdfResult Kdf(KdfParameterTls12 param, BitString fixedInfo = null)
        {
            var hashFunction = ShaAttributes.GetHashFunctionFromEnum(param.HashFunction);

            var kdf = _tlsFactory.GetTlsKdfInstance(TlsModes.v12, hashFunction);

            var result = kdf.DeriveKey(
                param.Z,
                param.InitiatorEphemeralData,
                param.ResponderEphemeralData,
                param.AdditionalInitiatorNonce,
                param.AdditionalResponderNonce,
                param.L);

            return new KdfResult(result.DerivedKey);
        }
    }
}
