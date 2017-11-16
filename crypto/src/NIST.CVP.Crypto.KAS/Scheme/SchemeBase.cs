using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBase<TSchemeParameters, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair> 
        : IScheme<TSchemeParameters, TParameterSet, TScheme, TOtherPartySharedInfo, TDomainParameters, TKeyPair>
        where TSchemeParameters : ISchemeParameters<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
        where TOtherPartySharedInfo : ISharedInformation<TDomainParameters, TKeyPair>
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        public abstract int OtherInputLength { get; }
        public TSchemeParameters SchemeParameters { get; protected set; }
        public FfcDomainParameters DomainParameters { get; protected set; }
        public FfcKeyPair StaticKeyPair { get; protected set; }
        public FfcKeyPair EphemeralKeyPair { get; protected set; }
        public BitString EphemeralNonce { get; protected set; }
        public BitString DkmNonce { get; protected set; }
        public BitString NoKeyConfirmationNonce { get; protected set; }

        public abstract void SetDomainParameters(FfcDomainParameters domainParameters);

        public abstract TOtherPartySharedInfo ReturnPublicInfoThisParty();

        public abstract KasResult ComputeResult(TOtherPartySharedInfo otherPartyInformation);
    }
}