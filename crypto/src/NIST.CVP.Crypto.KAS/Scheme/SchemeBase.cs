using System;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public abstract class SchemeBase<TSchemeParameters, TParameterSet, TScheme> 
        : IScheme<TSchemeParameters, TParameterSet, TScheme>
        where TSchemeParameters : ISchemeParameters<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
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

        public abstract FfcSharedInformation ReturnPublicInfoThisParty();

        public abstract KasResult ComputeResult(FfcSharedInformation otherPartyInformation);
    }
}