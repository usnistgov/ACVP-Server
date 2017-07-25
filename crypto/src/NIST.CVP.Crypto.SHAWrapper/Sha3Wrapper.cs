using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHAWrapper
{
    public class Sha3Wrapper : ISha
    {
        private readonly ISHA3Factory _iSha3Factory;
        private readonly SHA3.HashFunction _sha3HashFunction;
        
        public Sha3Wrapper(ISHA3Factory iSha3Factory, SHA3.HashFunction sha3HashFunction)
        {
            _iSha3Factory = iSha3Factory;
            _sha3HashFunction = sha3HashFunction;
        }
        
        public BitString HashMessage(BitString message)
        {
            var sha = _iSha3Factory.GetSHA(_sha3HashFunction);
            return sha.HashMessage(
                message, 
                _sha3HashFunction.DigestSize, 
                _sha3HashFunction.Capacity,
                _sha3HashFunction.XOF
            );
        }
    }
}
