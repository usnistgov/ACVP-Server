using System.Numerics;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
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