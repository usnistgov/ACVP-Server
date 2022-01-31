using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class HssPrivateKey
    {
        public LmsPrivateKey[] PrivateKeys { get; set; }
        public BitString[] PublicKeys { get; set; }
        public BitString[] Signatures { get; set; }

        public HssPrivateKey(LmsPrivateKey[] privateKeys, BitString[] publicKeys, BitString[] signatures)
        {
            PrivateKeys = new LmsPrivateKey[privateKeys.Length];
            for (int i = 0; i < PrivateKeys.Length; i++)
            {
                PrivateKeys[i] = privateKeys[i].GetDeepCopy();
            }

            PublicKeys = new BitString[publicKeys.Length];
            for (int i = 0; i < PublicKeys.Length; i++)
            {
                PublicKeys[i] = publicKeys[i].GetDeepCopy();
            }

            Signatures = new BitString[signatures.Length];
            for (int i = 0; i < Signatures.Length; i++)
            {
                Signatures[i] = signatures[i].GetDeepCopy();
            }
        }

        public HssPrivateKey GetDeepCopy()
        {
            return new HssPrivateKey(PrivateKeys, PublicKeys, Signatures);
        }
    }
}
