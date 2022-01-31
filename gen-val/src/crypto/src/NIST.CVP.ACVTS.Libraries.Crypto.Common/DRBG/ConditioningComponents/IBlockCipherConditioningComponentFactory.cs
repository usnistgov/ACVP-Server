namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents
{
    public interface IBlockCipherConditioningComponentFactory
    {
        IDrbgConditioningComponent GetInstance(int keyLength);
    }
}
