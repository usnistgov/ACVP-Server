namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public interface IKdfVisitor
    {
        KdfResult Kdf(KdfParameterOneStep param);
    }
}