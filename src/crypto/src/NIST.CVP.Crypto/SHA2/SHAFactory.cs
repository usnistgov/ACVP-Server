﻿using System;
using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Crypto.SHA2
{
    public class SHAFactory : ISHAFactory
    {
        public ISHABase GetSHA(HashFunction hashFunction)
        {
            var shaInternals = new SHAInternals(hashFunction);

            if (!IsValidHashFunction(hashFunction))
            {
                throw new ArgumentException($"Invalid hash function. Cannot combine {hashFunction.Mode} and {hashFunction.DigestSize}.");
            }

            if (hashFunction.Mode == ModeValues.SHA1)
            {
                return new SHA1(shaInternals);
            }
            else if (hashFunction.Mode == ModeValues.SHA2)
            {
                if (hashFunction.DigestSize == DigestSizes.d224 || hashFunction.DigestSize == DigestSizes.d256)
                {
                    return new SHA2Small(shaInternals);
                }
                else
                {
                    return new SHA2Large(shaInternals);
                }
            }
            else
            {
                throw new ArgumentException($"Invalid value for {nameof(hashFunction.Mode)}");
            }
        }

        private bool IsValidHashFunction(HashFunction hashFunction)
        {
            if(hashFunction.Mode == ModeValues.SHA1)
            {
                if(hashFunction.DigestSize != DigestSizes.d160)
                {
                    return false;
                }
            }
            else
            {
                if(hashFunction.DigestSize == DigestSizes.d160)
                {
                    return false;
                }
            }

            return true;
        }
    }
}