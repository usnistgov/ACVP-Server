using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines
{
    public class EngineInitParameters : IBlockCipherEngineParameters
    {
        public BlockCipherDirections Direction { get; }
        public byte[] Key { get; }
        public bool UseInverseCipherMode { get; }

        public EngineInitParameters(BlockCipherDirections direction, byte[] key, bool useInverseCipherMode = false)
        {
            Direction = direction;
            Key = key;
            UseInverseCipherMode = useInverseCipherMode;
        }
    }
}
