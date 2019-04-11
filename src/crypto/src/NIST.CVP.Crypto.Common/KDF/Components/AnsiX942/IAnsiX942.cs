using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.AnsiX942
{
    public interface IAnsiX942
    {
        KdfResult DeriveKey(BitString zz, int keyLen, BitString otherInfo);
    }
}
