namespace NIST.CVP.Crypto.Symmetric.Engines
{
    public interface IRijndaelInternals
    {
        void InvMixColumn(byte[,] block, int blockCount);
        void KeyAddition(byte[,] block, byte[,] roundKey, int blockCount);
        void MixColumn(byte[,] block, int blockCount);
        byte Multiply(byte a, byte b);
        void ShiftRow(byte[,] block, int d, int blockCount);
        void Substitution(byte[,] block, byte[] box, int blockCount);
    }
}