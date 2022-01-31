using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG
{
    public class DrbgResult
    {
        public BitString Bits { get; }
        public DrbgStatus DrbgStatus { get; }

        public DrbgResult(BitString bits)
        {
            Bits = bits;
            DrbgStatus = DrbgStatus.Success;
        }

        public DrbgResult(DrbgStatus drbgStatus)
        {
            DrbgStatus = drbgStatus;

            if (drbgStatus != DrbgStatus.Success)
            {
                Bits = new BitString(0);
            }
        }

        public bool Success => DrbgStatus == DrbgStatus.Success;

        public override string ToString()
        {
            if (!Success)
            {
                return DrbgStatus.ToString();
            }
            return $"Bits: {Bits.ToHex()}";
        }
    }
}
