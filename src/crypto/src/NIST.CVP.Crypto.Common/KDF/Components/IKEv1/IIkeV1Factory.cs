using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;

namespace NIST.CVP.Crypto.Common.KDF.Components.IKEv1
{
    public interface IIkeV1Factory
    {
        IIkeV1 GetIkeV1Instance(AuthenticationMethods authMethods, HashFunction hash);
    }
}
