using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.XTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.XTS
{
    public class XtsModeBlockCipherFactory : IXtsModeBlockCipherFactory
    {
        public IXtsModeBlockCipher GetXtsCipher()
        {
            return new XtsBlockCipher(new AesEngine());
        }
    }
}
