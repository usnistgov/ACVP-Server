using NIST.CVP.Crypto.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class TDESBlockInput : IBlockInput
    {
        public TDESBlockInput(BitString key, BitString iv, BitString data)
        {
            Key = key;
            Iv = iv;
            Data = data;
        }
        public BitString Key { get; set; }
        public BitString Iv { get; set; }
        public BitString Data { get; set; }
    }
}