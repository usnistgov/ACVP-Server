namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTS
{
    public interface ICiphertextStealingTransformFactory
    {
        ICiphertextStealingTransform Get(CiphertextStealingMode mode);
    }
}
