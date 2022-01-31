using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public interface IHss
    {
        BitString SEED { get; set; }
        BitString RootI { get; set; }

        Task<HssKeyPair> GenerateHssKeyPairAsync();
        Task<HssKeyPair> UpdateKeyPairOneStepAsync(HssKeyPair keyPair);
        Task<HssSignatureResult> GenerateHssSignatureAsync(BitString msg, HssKeyPair keyPair, int advanced = 0);
        HssVerificationResult VerifyHssSignature(BitString msg, BitString publicKey, BitString signature);
    }
}
