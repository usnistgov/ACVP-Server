using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.DRBG
{
    public class DrbgResult
    {
        public BitString Bits { get; private set; }
        public DrbgStatus DrbgStatus { get; private set; }

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

        public bool Success
        {
            get { return DrbgStatus == DrbgStatus.Success; }
        }

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