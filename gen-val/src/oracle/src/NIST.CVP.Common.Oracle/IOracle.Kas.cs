﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KAS.SafePrimes.Enums;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<FfcDomainParameters> GetSafePrimeGroupsDomainParameterAsync(SafePrime param);
        
        Task<KasValResult> GetKasValTestAsync(KasValParameters param);
        Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param);
        Task<ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param);
    
        Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param);
        Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param);
        Task<ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param);
        
        Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param);
        Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param);
        Task<ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param);

        Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param);
        Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param);
        Task<ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param);
        
        Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param);
        Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param);

        Task<SafePrimesKeyVerResult> GetSafePrimesKeyVerTestAsync(SafePrimesKeyVerParameters param);
    }
}