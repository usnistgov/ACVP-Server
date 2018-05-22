namespace NIST.CVP.Crypto.Common.Symmetric.AES
{
    public interface IRijndaelInternals
    {
        void EncryptSingleBlock(byte[,] block, Key key);
        /// <summary>
        /// Xor corresponding text input and round key input bytes
        /// </summary>
        /// <param name="block"></param>
        /// <param name="roundKey"></param>
        /// <param name="blockCount"></param>
        void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount);
        /// <summary>
        /// Replace every byte of the input by the byte at that place in the nonlinear S-box
        /// </summary>
        /// <param name="block"></param>
        /// <param name="box"></param>
        /// <param name="blockCount"></param>
        void Substitution(byte[,] block, byte[] box, int blockCount);
        /// <summary>
        /// Row 0 remains unchanged, The other three rows are shifted a variable amount
        /// </summary>
        /// <param name="block"></param>
        /// <param name="d"></param>
        /// <param name="blockCount"></param>
        void ShiftRow(byte[,] block, int d, int blockCount);
        /// <summary>
        /// Mix the four bytes of every column in a linear way
        /// </summary>
        /// <param name="block">Block to mix</param>
        /// <param name="blockCount">Number of blocks</param>
        void MixColumn(byte[,] block, int blockCount);
        /// <summary>
        /// Inverse of <see cref="MixColumn"/>
        /// </summary>
        /// <param name="block">Block to mix</param>
        /// <param name="blockCount">Number of blocks</param>
        void InvMixColumn(byte[,] block, int blockCount);
        byte Multiply(byte a, byte b);
    }
}
