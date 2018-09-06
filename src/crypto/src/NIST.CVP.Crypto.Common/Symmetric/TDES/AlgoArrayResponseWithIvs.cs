using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric.TDES
{
    public class AlgoArrayResponseWithIvs : AlgoArrayResponse
    {

        public BitString IV1 { get; set; }
        public BitString IV2 { get; set; }
        public BitString IV3 { get; set; }

        public AlgoArrayResponseWithIvs() { }

        public AlgoArrayResponseWithIvs(string key, string plaintext, string ciphertext, string iv1, string iv2, string iv3) 
            : base(key, plaintext, ciphertext)
        {
            IV1 = new BitString(iv1);
            IV2 = new BitString(iv2);
            IV3 = new BitString(iv3);
        }
    }
}