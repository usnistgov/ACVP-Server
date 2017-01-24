using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public interface IAES_OFB_MCT
    {
        MCTResult MCTEncrypt(BitString iv, BitString key, BitString plainText);
        MCTResult MCTDecrypt(BitString iv, BitString key, BitString cipherText);
    }
}
