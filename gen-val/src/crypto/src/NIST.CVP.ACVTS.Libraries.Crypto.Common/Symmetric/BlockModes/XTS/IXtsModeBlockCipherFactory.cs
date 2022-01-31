namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS
{
    public interface IXtsModeBlockCipherFactory
    {
        IXtsModeBlockCipher GetXtsCipher();    // Should always be fixed at AES-XTS for engine and mode.
    }
}
