using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.TLS
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
