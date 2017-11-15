using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.Scheme
{
    public interface ISchemeParameters<out TParameterSet, out TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {

        TParameterSet ParameterSet { get; }
        KasAssurance KasAssurances { get; }
        KasMode KasMode { get; }
        KeyAgreementRole KeyAgreementRole { get; }
        KeyConfirmationDirection KeyConfirmationDirection { get; }
        KeyConfirmationRole KeyConfirmationRole { get; }
        TScheme Scheme { get; }
        BitString ThisPartyId { get; }
    }
}