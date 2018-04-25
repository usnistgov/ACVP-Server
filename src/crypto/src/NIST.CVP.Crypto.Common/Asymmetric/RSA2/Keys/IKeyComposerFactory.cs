using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys
{
    public interface IKeyComposerFactory
    {
        IRsaKeyComposer GetKeyComposer(PrivateKeyModes privateKeyModes);
    }
}