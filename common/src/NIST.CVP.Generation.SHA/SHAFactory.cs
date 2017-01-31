using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA
{
    public class SHAFactory : ISHAFactory
    {
        public SHA GetSHA(HashFunction hashFunction)
        {
            var shaInternals = new SHAInternals(hashFunction);

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
    }
}
