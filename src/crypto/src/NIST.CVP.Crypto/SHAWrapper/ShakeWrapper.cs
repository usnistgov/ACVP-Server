using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class ShakeWrapper : ISha
    {
        private readonly ISHA3Factory _iSha3Factory;
        private readonly Common.Hash.SHA3.HashFunction _mappedHashFunction;
        private
            List<(string algo, ModeValues wrapperMode, DigestSizes wrapperSizes, int mappedDigestSize)> ShakeMappings =
                new List<(string algo, ModeValues wrapperMode, DigestSizes wrapperSizes, int mappedDigestSize)>()
                {
                    ("SHAKE-128", ModeValues.SHAKE, DigestSizes.d128, 128),
                    ("SHAKE-256", ModeValues.SHAKE, DigestSizes.d256, 256),
                };

        public Common.Hash.ShaWrapper.HashFunction HashFunction { get; }

        public ShakeWrapper(ISHA3Factory iSha3Factory, Common.Hash.ShaWrapper.HashFunction hashFunction)
        {
            _iSha3Factory = iSha3Factory;
            _mappedHashFunction = ToSha3HashFunction(hashFunction);
            HashFunction = hashFunction;
        }

        public HashResult HashMessage(BitString message, int outLen = 0)
        {
            try
            {
                var sha = _iSha3Factory.GetSHA(_mappedHashFunction);
                return new HashResult(
                    sha.HashMessage(
                        message,
                        outLen,
                        _mappedHashFunction.Capacity,
                        _mappedHashFunction.XOF
                    )
                );
            }
            catch (Exception ex)
            {
                return new HashResult(ex.Message);
            }
        }

        public HashResult HashNumber(BigInteger number)
        {
            throw new NotImplementedException();
        }

        private Common.Hash.SHA3.HashFunction ToSha3HashFunction(Common.Hash.ShaWrapper.HashFunction wrapperHashFunction)
        {
            if (!ShakeMappings
                .TryFirst(
                    w => w.wrapperMode == wrapperHashFunction.Mode && w.wrapperSizes == wrapperHashFunction.DigestSize,
                    out var result))
            {
                throw new ArgumentException($"Invalid {nameof(wrapperHashFunction)}.");
            }

            Common.Hash.SHA3.HashFunction hashFunction = new Common.Hash.SHA3.HashFunction()
            {
                DigestSize = result.mappedDigestSize,
                Capacity = result.mappedDigestSize * 2,
                XOF = true
            };

            return hashFunction;
        }
    }
}
