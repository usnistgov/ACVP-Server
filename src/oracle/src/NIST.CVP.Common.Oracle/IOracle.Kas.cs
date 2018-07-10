using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KasValResultEcc GetKasValTestEcc(KasValParametersEcc param);
        KasAftResultEcc GetKasAftTestEcc(KasAftParametersEcc param);
    }
}
