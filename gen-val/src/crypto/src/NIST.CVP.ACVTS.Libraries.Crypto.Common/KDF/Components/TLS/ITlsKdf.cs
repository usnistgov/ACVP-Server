using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS
{
    public interface ITlsKdf
    {
        TlsKdfResult DeriveKey(
            BitString preMasterSecret,
            BitString clientHelloRandom,
            BitString serverHelloRandom,
            BitString clientRandom,
            BitString serverRandom,
            int keyBlockLength
        );
    }
}
