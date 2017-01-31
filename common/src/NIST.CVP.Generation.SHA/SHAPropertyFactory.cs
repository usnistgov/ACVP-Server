using NIST.CVP.Generation.SHA.SHAProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA
{
    public class SHAPropertyFactory
    {
        public SHAPropertiesBase GetSHAProperties(HashFunction hashFunction)
        {
            switch (hashFunction.DigestSize)
            {
                case DigestSizes.d160:
                    return new SHA160Properties();

                case DigestSizes.d224:
                    return new SHA224Properties();

                case DigestSizes.d256:
                    return new SHA256Properties();

                case DigestSizes.d384:
                    return new SHA384Properties();

                case DigestSizes.d512:
                    return new SHA512Properties();

                case DigestSizes.d512t224:
                    return new SHA512t224Properties();

                case DigestSizes.d512t256:
                    return new SHA512t256Properties();

                default:
                    throw new Exception("Improper digest size. No matching properties class.");
            }
        }
    }
}
