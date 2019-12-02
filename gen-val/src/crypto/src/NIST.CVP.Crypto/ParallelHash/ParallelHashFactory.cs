using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;

namespace NIST.CVP.Crypto.ParallelHash
{
    public class ParallelHashFactory : IParallelHashFactory
    {
        public IParallelHashWrapper GetParallelHash(HashFunction hashFunction)
        {
            var errors = IsValidHashFunction(hashFunction);
            if (!string.IsNullOrEmpty(errors))
            {
                throw new ArgumentException($"Invalid hash function. {errors}");
            }

            return new ParallelHashWrapper();
        }

        private string IsValidHashFunction(HashFunction hashFunction)
        {
            var errors = new List<string>();

            var capacity = hashFunction.Capacity;
            var digSize = hashFunction.DigestLength;

            if (capacity != 128 * 2 && capacity != 256 * 2)
            {
                errors.Add("Incorrect capacity size for XOF");
            }

            if (digSize < 16 || digSize > 65536)
            {
                errors.Add("Digest size must be between 16 and 65536 (2^16)");
            }

            return string.Join("; ", errors);
        }
    }
}
