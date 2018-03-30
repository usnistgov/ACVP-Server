using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public class SymmetricCipherWithIvResult : SymmetricCipherResult
    {
        public BitString[] IVs { get; set; }
        public BitString[] Results { get; set; }

        public SymmetricCipherWithIvResult(BitString result, BitString[] ivs) : base(result)
        {
            IVs = ivs;
        }

        public SymmetricCipherWithIvResult(BitString result, BitString[] ivs, BitString[] results) : base(result)
        {
            IVs = ivs;
            Results = results;
        }

        public SymmetricCipherWithIvResult(string errorMessage) : base(errorMessage)
        {
        }
    }
}