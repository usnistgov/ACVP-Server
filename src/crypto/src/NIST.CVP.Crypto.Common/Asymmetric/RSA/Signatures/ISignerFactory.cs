namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public interface ISignerFactory
    {
        ISignerBase GetSigner(string type);
    }
}