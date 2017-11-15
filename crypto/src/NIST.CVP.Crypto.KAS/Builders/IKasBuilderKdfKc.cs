using System;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfKc<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        IKas<TParameterSet, TScheme> Build();
        IKasBuilderKdfKc<TParameterSet, TScheme> WithKeyConfirmationDirection(KeyConfirmationDirection value);
        IKasBuilderKdfKc<TParameterSet, TScheme> WithKeyConfirmationRole(KeyConfirmationRole value);
        IKasBuilderKdfKc<TParameterSet, TScheme> WithKeyLength(int value);
        IKasBuilderKdfKc<TParameterSet, TScheme> WithMacParameters(MacParameters value);
        IKasBuilderKdfKc<TParameterSet, TScheme> WithOtherInfoPattern(string value);
    }
}