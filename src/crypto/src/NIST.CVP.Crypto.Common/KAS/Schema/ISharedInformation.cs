using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.Schema
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