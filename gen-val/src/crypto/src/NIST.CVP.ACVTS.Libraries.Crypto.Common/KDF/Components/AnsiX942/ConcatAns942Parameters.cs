using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942
{
    public class ConcatAns942Parameters : IAns942Parameters
    {
        public BitString Zz { get; set; }
        public int KeyLen { get; set; }
        public BitString OtherInfo { get; set; }
    }
}
