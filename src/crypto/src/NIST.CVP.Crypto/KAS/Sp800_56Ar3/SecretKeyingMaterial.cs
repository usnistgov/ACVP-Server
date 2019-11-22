using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Math;

internal class SecretKeyingMaterial : ISecretKeyingMaterial
{
    public IDsaKeyPair StaticKeyPair { get; set; }
    public IDsaKeyPair EphemeralKeyPair { get; set; }
    public BitString EphemeralNonce { get; set; }
    public BitString DkmNonce { get; set; }
}