namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

public class KeyPair
{
    public PrivateKey PrivateKey { get; set; }
    public PublicKey PublicKey { get; set; }

    public KeyPair(PrivateKey privateKey, PublicKey publicKey)
    {
        PrivateKey = privateKey;
        PublicKey = publicKey;
    }
}
