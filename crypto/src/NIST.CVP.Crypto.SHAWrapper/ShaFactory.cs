using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class ShaFactory : IShaFactory
    {
        public ISha GetShaInstance(HashFunction hashFunction)
        {
            switch (hashFunction.Mode)
            {
                case ModeValues.SHA1:
                case ModeValues.SHA2:
                    return new Sha2Wrapper(new SHAFactory(), hashFunction);
                case ModeValues.SHA3:
                    return new Sha3Wrapper(new SHA3Factory(), hashFunction);
                default:
                    throw new ArgumentException($"{nameof(hashFunction)}");
            }
        }

        

        

        

        
    }
}
