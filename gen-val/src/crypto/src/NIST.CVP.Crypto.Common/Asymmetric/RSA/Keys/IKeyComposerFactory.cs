using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IKeyComposerFactory
    {
        IRsaKeyComposer GetKeyComposer(PrivateKeyModes privateKeyModes);
    }
}