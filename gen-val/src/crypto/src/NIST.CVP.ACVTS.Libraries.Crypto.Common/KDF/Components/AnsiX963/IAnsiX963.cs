using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX963
{
    public interface IAnsiX963
    {
        KdfResult DeriveKey(BitString z, BitString sharedInfo, int keyLength);
    }
}
