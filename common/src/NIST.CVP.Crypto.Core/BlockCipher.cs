namespace NIST.CVP.Crypto.Core
{
    public abstract class BlockCipher : ICipher
    {
        public abstract int BlockSize { get;  }
    }
}