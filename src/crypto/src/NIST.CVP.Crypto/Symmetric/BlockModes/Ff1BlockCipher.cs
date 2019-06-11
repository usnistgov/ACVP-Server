using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.BlockModes
{
    public class Ff1BlockCipher : ModeBlockCipherBase<SymmetricCipherResult>
    {
        public Ff1BlockCipher(IBlockCipherEngine engine) : base(engine)
        {
        }

        public override bool IsPartialBlockAllowed => true;
        public override SymmetricCipherResult ProcessPayload(IModeBlockCipherParameters param)
        {
            throw new System.NotImplementedException();
        }
    }
}