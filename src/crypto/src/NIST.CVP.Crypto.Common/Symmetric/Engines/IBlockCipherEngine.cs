using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;

namespace NIST.CVP.Crypto.Common.Symmetric.Engines
{
    /// <summary>
    /// Interface for symmetric block cipher primitive operations
    /// </summary>
    public interface IBlockCipherEngine
    {
        /// <summary>
        /// The cipher's block size in bytes
        /// </summary>
        /// <returns></returns>
        int BlockSizeBytes { get; }
        /// <summary>
        /// The cipher's block size in bits
        /// </summary>
        int BlockSizeBits { get; }
        /// <summary>
        /// The underlying engine cipher
        /// TODO consolidate enums
        /// </summary>
        Cipher Cipher { get; }
        /// <summary>
        /// Initialize the primitive
        /// </summary>
        /// <param name="param"></param>
        void Init(IBlockCipherEngineParameters param);
        /// <summary>
        /// Process a single block of data
        /// </summary>
        /// <param name="payLoad">The data to process</param>
        /// <param name="outBuffer">The processed output</param>
        /// <param name="blockIndex">The block to process</param>
        /// 
        void ProcessSingleBlock(byte[] payLoad, byte[] outBuffer, int blockIndex);
    }
}