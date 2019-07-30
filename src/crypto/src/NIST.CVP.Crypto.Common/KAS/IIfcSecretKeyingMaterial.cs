using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS
{
    public interface IIfcSecretKeyingMaterial
    {
        KeyPair Key { get; }
        BitString DkmNonce { get; }
        BitString PartyId { get; }
    }
}