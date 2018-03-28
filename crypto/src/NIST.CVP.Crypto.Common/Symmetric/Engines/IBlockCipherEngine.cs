namespace NIST.CVP.Crypto.Common.Symmetric.Engines
{
    /// <summary>
    /// Interface for symmetric block cipher primitive operations
    /// </summary>
    public interface IBlockCipherEngine
    {
        /// <summary>
        /// The cipher's block size
        /// </summary>
        /// <returns></returns>
        int BlockSizeBits { get; }
        /// <summary>
        /// Initialize the primitive
        /// </summary>
        /// <param name="param"></param>
        void Init(IBlockCipherEngineParameters param);
        /// <summary>
        /// Process a single block of data
        /// </summary>
        /// <param name="block"></param>
        void ProcessSingleBlock(byte[,] block);
    }
}