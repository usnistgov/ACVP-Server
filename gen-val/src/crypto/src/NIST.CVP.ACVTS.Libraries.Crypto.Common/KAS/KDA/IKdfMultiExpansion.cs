namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA
{
    public interface IKdfMultiExpansion
    {
        KdfMultiExpansionResult DeriveKey(IKdfMultiExpansionParameter param);
    }
}
