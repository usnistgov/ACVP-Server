using System;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public abstract class KasBuilderNoKdfNoKc<TParameterSet, TScheme> : IKasBuilderNoKdfNoKc<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        protected readonly ISchemeBuilder<TParameterSet, TScheme> _schemeBuilder;
        protected readonly KeyAgreementRole _keyAgreementRole;
        protected readonly TScheme _scheme;
        protected readonly TParameterSet _parameterSet;
        protected readonly KasAssurance _assurances;
        protected readonly BitString _partyId;

        protected KasBuilderNoKdfNoKc(
            ISchemeBuilder<TParameterSet, TScheme> schemeBuilder, 
            KeyAgreementRole keyAgreementRole, 
            TScheme scheme, 
            TParameterSet parameterSet, 
            KasAssurance assurances, 
            BitString partyId
        )
        {
            _schemeBuilder = schemeBuilder;
            _keyAgreementRole = keyAgreementRole;
            _scheme = scheme;
            _parameterSet = parameterSet;
            _assurances = assurances;
            _partyId = partyId;
        }

        public abstract IKas<TParameterSet, TScheme> Build();
    }
}