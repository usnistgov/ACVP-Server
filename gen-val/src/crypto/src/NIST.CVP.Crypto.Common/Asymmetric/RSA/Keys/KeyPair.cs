namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public class KeyPair
    {
        public PublicKey PubKey { get; set; }
        public PrivateKeyBase PrivKey { get; set; } = new PrivateKey();
    }
}
