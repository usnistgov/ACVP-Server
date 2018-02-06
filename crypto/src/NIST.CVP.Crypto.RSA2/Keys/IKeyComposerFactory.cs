using NIST.CVP.Crypto.RSA2.Enums;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public interface IKeyComposerFactory
    {
        IRsaKeyComposer GetKeyComposer(PrivateKeyModes privateKeyModes);
    }
}