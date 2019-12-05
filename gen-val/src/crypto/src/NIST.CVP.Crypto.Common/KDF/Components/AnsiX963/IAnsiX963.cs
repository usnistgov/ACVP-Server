using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.AnsiX963
{
    public interface IAnsiX963
    {
        KdfResult DeriveKey(BitString z, BitString sharedInfo, int keyLength);
    }
}
