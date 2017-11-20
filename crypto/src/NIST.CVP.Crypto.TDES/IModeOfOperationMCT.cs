using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public interface IModeOfOperationMCT
    {   
        Algo Algo { get; }
        MCTResult<AlgoArrayResponse> MCTEncrypt(BitString key, BitString iv, BitString plainText);
        MCTResult<AlgoArrayResponse> MCTDecrypt(BitString key, BitString iv, BitString cipherText);
    }
}
