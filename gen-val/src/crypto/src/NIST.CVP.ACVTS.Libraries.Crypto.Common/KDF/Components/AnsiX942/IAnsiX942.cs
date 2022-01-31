using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942
{
    public interface IAnsiX942
    {
        KdfResult DeriveKey(IAns942Parameters param);
    }
}
