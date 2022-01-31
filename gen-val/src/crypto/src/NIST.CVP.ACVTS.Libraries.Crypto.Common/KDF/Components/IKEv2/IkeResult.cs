using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2
{
    public class IkeResult
    {
        public BitString SKeySeed { get; set; }
        public BitString DKM { get; set; }
        public BitString DKMChildSA { get; set; }
        public BitString DKMChildSADh { get; set; }
        public BitString SKeySeedReKey { get; set; }
        public string ErrorMessage { get; set; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public IkeResult(BitString sKeySeed, BitString dkm, BitString dkmChildSA, BitString dkmChildSADh, BitString sKeySeedReKey)
        {
            SKeySeed = sKeySeed;
            DKM = dkm;
            DKMChildSA = dkmChildSA;
            DKMChildSADh = dkmChildSADh;
            SKeySeedReKey = sKeySeedReKey;
        }

        public IkeResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
