using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942
{
    public interface IAns942Parameters
    {
        BitString Zz { get; }
        int KeyLen { get; set; }
    }
}
