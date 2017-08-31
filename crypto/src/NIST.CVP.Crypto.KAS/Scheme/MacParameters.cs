using System;
using System.Diagnostics.Tracing;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public class MacParameters
    {
        public KeyAgreementMacType MacType { get; }
        public HashFunction HashFunction { get; }
        public BitString Nonce { get; }

        public MacParameters(KeyAgreementMacType macType, HashFunction hashFunction)
        {
            if (macType == KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            MacType = macType;
            HashFunction = hashFunction;
        }

        public MacParameters(KeyAgreementMacType macType, HashFunction hashFunction, BitString nonce)
        {
            if (macType != KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }
            if (nonce == null || nonce.BitLength == 0)
            {
                throw new ArgumentException(nameof(nonce));
            }

            MacType = macType;
            HashFunction = hashFunction;
            Nonce = nonce;
        }
    }
}