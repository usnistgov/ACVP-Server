using System;
using System.Security.Cryptography;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.HMAC
{
    public class FastHmac : IHmac
    {
        private readonly ISha _iSha;

        public int OutputLength => _iSha.HashFunction.OutputLen;

        public FastHmac(ISha iSha)
        {
            _iSha = iSha;
        }
        
        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {
            var hmac = GetHmac(key);
            var result = hmac.ComputeHash(message.ToBytes());
            return new MacResult(new BitString(result));
        }

        private System.Security.Cryptography.HMAC GetHmac(BitString key)
        {
            var keyBytes = key.ToBytes();
            switch (_iSha.HashFunction.Mode)
            {
                case ModeValues.SHA1:
                    return new HMACSHA1(keyBytes);
                case ModeValues.SHA2:
                    switch (_iSha.HashFunction.DigestSize)
                    {
                        case DigestSizes.d256:
                            return new HMACSHA256(keyBytes);
                        case DigestSizes.d384:
                            return new HMACSHA384(keyBytes);
                        case DigestSizes.d512:
                            return new HMACSHA512(keyBytes);
                        default:
                            throw new Exception();
                    }
                default:
                    throw new Exception();
            }
        }
    }
}