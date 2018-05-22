namespace NIST.CVP.Crypto.Common.Asymmetric.RSA
{
    public interface IKeyPair
    {
        PrivateKey PrivKey { get; set; }
        PublicKey PubKey { get; set; }
    }
}