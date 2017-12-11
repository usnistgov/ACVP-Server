using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.DRBG.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using ModeValues = NIST.CVP.Crypto.SHAWrapper.ModeValues;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgFactory : IDrbgFactory
    {
        public IDrbg GetDrbgInstance(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider)
        {
            switch (drbgParameters.Mechanism)
            {
                case DrbgMechanism.Counter:
                    return GetCounterImplementation(drbgParameters, iEntropyProvider);
                case DrbgMechanism.Hash:
                    return GetHashImplementation(drbgParameters, iEntropyProvider);
                default:
                    throw new ArgumentException("Invalid DRBG Mechanism provided");
            }
        }

        private IDrbg GetCounterImplementation(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider)
        {
            var aesEcb = new AES_ECB.AES_ECB(new RijndaelFactory(new RijndaelInternals()));
            var tdesEcb = new TDES_ECB.TDES_ECB();

            switch (drbgParameters.Mode)
            {
                case DrbgMode.AES128:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters);
                case DrbgMode.AES192:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters);
                case DrbgMode.AES256:
                    return new DrbgCounterAes(iEntropyProvider, aesEcb, drbgParameters);
                case DrbgMode.TDES:
                    return new DrbgCounterTdes(iEntropyProvider, tdesEcb, drbgParameters);
                default:
                    throw new ArgumentException("Invalid DRBG mode provided for current mechanism");
            }
        }

        private IDrbg GetHashImplementation(DrbgParameters drbgParameters, IEntropyProvider iEntropyProvider)
        {
            var hashFunction = GetHashFunction(drbgParameters.Mode);
            var sha = new ShaFactory().GetShaInstance(hashFunction);
            return new DrbgHash(iEntropyProvider, sha, drbgParameters);
        }

        private HashFunction GetHashFunction(DrbgMode mode)
        {
            switch (mode)
            {
                case DrbgMode.SHA1:
                    return new HashFunction(ModeValues.SHA1, DigestSizes.d160);
                case DrbgMode.SHA224:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d224);
                case DrbgMode.SHA256:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                case DrbgMode.SHA384:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d384);
                case DrbgMode.SHA512:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d512);
                case DrbgMode.SHA512t224:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d512t224);
                case DrbgMode.SHA512t256:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d512t256);
                default:
                    throw new ArgumentException("Invalid DRBG mode provided for current mechanism");
            }
        }
    }
}

