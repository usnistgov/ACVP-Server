﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<MacResult> GetCmacCaseAsync(CmacParameters param);
        Task<MacResult> GetHmacCaseAsync(HmacParameters param);
        Task<KmacResult> GetKmacCaseAsync(KmacParameters param);
    }
}