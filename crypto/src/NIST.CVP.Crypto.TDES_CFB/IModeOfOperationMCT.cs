using NIST.CVP.Crypto.Core;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public interface IModeOfOperationMCT
    {   
        Algo Algo { get; }
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString iv, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}
