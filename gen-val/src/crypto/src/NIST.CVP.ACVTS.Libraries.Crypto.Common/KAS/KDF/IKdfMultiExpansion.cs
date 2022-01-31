namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    public interface IKdfMultiExpansion
    {
        KdfMultiExpansionResult DeriveKey(IKdfMultiExpansionParameter param);
    }
}
