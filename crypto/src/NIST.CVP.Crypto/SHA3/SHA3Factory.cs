using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.SHA3
{
    public class SHA3Factory : ISHA3Factory
    {
        public ISHA3Wrapper GetSHA(HashFunction hashFunction)
        {
            var errors = IsValidHashFunction(hashFunction);
            if (!string.IsNullOrEmpty(errors))
            {
                throw new ArgumentException($"Invalid hash function. {errors}");
            }

            return new SHA3Wrapper();
        }

        private string IsValidHashFunction(HashFunction hashFunction)
        {
            var errors = new List<string>();

            var capacity = hashFunction.Capacity;
            var digSize = hashFunction.DigestSize;
            if (hashFunction.XOF)
            {
                if (capacity != 128 * 2 && capacity != 256 * 2)
                {
                    errors.Add("Incorrect capacity size for XOF");
                }

                if (digSize < 16 || digSize > 65536)
                {
                    errors.Add("Digest size must be between 16 and 65536 (2^16)");
                }
            }
            else
            {
                if (capacity != 224 * 2 && capacity != 256 * 2 && capacity != 384 * 2 && capacity != 512 * 2)
                {
                    errors.Add("Incorrect capacity size. Must be 2 * digest size");
                }

                if (digSize != 224 && digSize != 256 && digSize != 384 && digSize != 512)
                {
                    errors.Add("Incorrect digest size. Must be one of: 224, 256, 384, 512");
                }
            }

            return string.Join("; ", errors);
        }
    }
}
