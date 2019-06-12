namespace NIST.CVP.Crypto.Common.Symmetric.BlockModes.Ffx
{
    public interface IFfxModeBlockCipherParameters : IModeBlockCipherParameters
    {
        /// <summary>
        /// The number base.
        /// </summary>
        short Radix { get; }
    }
}