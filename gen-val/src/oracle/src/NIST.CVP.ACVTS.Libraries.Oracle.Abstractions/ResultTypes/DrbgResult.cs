using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
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
