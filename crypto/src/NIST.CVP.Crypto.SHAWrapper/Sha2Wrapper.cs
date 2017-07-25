using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class Sha2Wrapper : ISha
    {
        private readonly ISHAFactory _shaFactory;
        private readonly SHA2.HashFunction _hashFunction;

        public Sha2Wrapper(ISHAFactory shaFactory, SHA2.HashFunction hashFunction)
        {
            _shaFactory = shaFactory;
            _hashFunction = hashFunction;
        }
        
        public BitString HashMessage(BitString message)
        {
            var sha = _shaFactory.GetSHA(_hashFunction);
            return sha.HashMessage(message);
        }
    }
}
