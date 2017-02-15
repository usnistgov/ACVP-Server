using System;

namespace NIST.CVP.Generation.SHA2
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

            switch (hashFunction.Mode)
            {
                case ModeValues.SHA1:
                    return new SHA1(shaInternals);
                case ModeValues.SHA2:
                    return new SHA2(shaInternals);
                default:
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
