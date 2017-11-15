using System;

namespace NIST.CVP.Crypto.KAS.Builders
{
    public interface IKasBuilderNoKdfNoKc<TParameterSet, TScheme>
        where TParameterSet : struct, IComparable
        where TScheme : struct, IComparable
    {
        IKas<TParameterSet, TScheme> Build();
    }
}