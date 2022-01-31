namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS
{
    public interface IXtsModeBlockCipher
    {
        SymmetricCipherResult ProcessPayload(IXtsModeBlockCipherParameters param);
    }
}
