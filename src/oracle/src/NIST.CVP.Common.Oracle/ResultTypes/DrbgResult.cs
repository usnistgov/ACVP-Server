using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.DRBG.Enums;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DrbgResult
    {
        public BitString EntropyInput { get; set; }
        public BitString Nonce { get; set; }
        public BitString PersoString { get; set; }
        public List<OtherInput> OtherInput { get; set; }

        public BitString ReturnedBits { get; set; }
        public DrbgStatus Status { get; set; }
    }
}
