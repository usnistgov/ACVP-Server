using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public interface IAES_OFB_MCT
    {
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
