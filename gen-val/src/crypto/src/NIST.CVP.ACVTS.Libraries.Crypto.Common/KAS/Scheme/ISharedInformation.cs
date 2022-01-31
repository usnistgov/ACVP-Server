using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    public interface ISharedInformation<out TDomainParameters, out TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        BitString DkmNonce { get; }
        TDomainParameters DomainParameters { get; }
        BitString EphemeralNonce { get; }
        TKeyPair StaticPublicKey { get; }
        TKeyPair EphemeralPublicKey { get; }
        BitString NoKeyConfirmationNonce { get; }
        BitString PartyId { get; }

    }
}
