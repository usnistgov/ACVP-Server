using System;
using NIST.CVP.Crypto.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderKdfNoKc<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        IKas<TParameterSet, TScheme> Build();
        IKasBuilderKdfNoKc<TParameterSet, TScheme> WithKeyLength(int value);
        IKasBuilderKdfNoKc<TParameterSet, TScheme> WithMacParameters(MacParameters value);
        IKasBuilderKdfNoKc<TParameterSet, TScheme> WithOtherInfoPattern(string value);
    }
}