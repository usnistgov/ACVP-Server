namespace NIST.CVP.Crypto.Common.Symmetric.CTS
{
    public interface ICiphertextStealingTransformFactory
    {
        ICiphertextStealingTransform Get(CiphertextStealingMode mode);
    }
}