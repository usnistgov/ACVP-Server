namespace NIST.CVP.Crypto.Symmetric.BlockModes.ShiftRegister
{
    /// <summary>
    /// Strategy interface for utilizing a shift register along with CFB Mode.
    /// </summary>
    public interface IShiftRegisterStrategy
    {
        /// <summary>
        /// The shift size - 1 bite, 8 bits (1 byte), or the block size in bits
        /// </summary>
        int ShiftSize { get; }
        /// <summary>
        /// Given the outbuffer (block encrypted IV),
        /// XOR the payload for the specified segment into position 0 of the outbuffer
        /// </summary>
        /// <param name="payload">The payload</param>
        /// <param name="segmentNumber">The segment of the payload to use</param>
        /// <param name="ivOutBuffer">The encrypted iv that will have payload[segment] XORed into position 0 (modified in impl)</param>
        void SetSegmentInProcessedBlock(byte[] payload, int segmentNumber, byte[] ivOutBuffer);
        /// <summary>
        /// Sets the current segment's outBuffer information based on processed block.
        /// </summary>
        /// <param name="block">The block to process for next rounds IV</param>
        /// <param name="segmentNumber">The segment number of the outBuffer to set</param>
        /// <param name="outBuffer">The out buffer (modified in impl)</param>
        void SetOutBufferSegmentFromIvXorPayload(byte[] block, int segmentNumber, byte[] outBuffer);
        /// <summary>
        /// Sets up next round's IV..
        /// </summary>
        /// <param name="block">The block to process for next round's IV</param>
        /// <param name="segmentNumber">The segment number of the block</param>
        /// <param name="iv">The IV to manipulate for next round</param>
        void SetNextRoundIv(byte[] block, int segmentNumber, byte[] iv);
    }
}