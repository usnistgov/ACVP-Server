namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public interface IKdfVisitable
    {
        KdfResult AcceptKdf(IKdfVisitor visitor);
    }
}