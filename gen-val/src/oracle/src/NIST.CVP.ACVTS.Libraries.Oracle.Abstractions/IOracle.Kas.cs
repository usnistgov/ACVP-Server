using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Cr1;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<KasValResult> GetKasValTestAsync(KasValParameters param, bool firstAttempt = false);
        Task<KasAftResult> GetKasAftTestAsync(KasAftParameters param);
        Task<ResultTypes.Kas.Sp800_56Ar3.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParameters param);

        Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param);
        Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param);
        Task<ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param);

        Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param);
        Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param);
        Task<ResultTypes.Kas.Sp800_56Ar1.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param);

        Task<KasValResultIfc> GetKasValTestIfcAsync(KasValParametersIfc param, KeyPair serverKey, KeyPair iutKey);
        Task<KasAftResultIfc> GetKasAftTestIfcAsync(KasAftParametersIfc param, KeyPair serverKey, KeyPair iutKey);
        Task<ResultTypes.Kas.Sp800_56Br2.KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersIfc param);

        Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param);
        Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param);

        Task<SafePrimesKeyVerResult> GetSafePrimesKeyVerTestAsync(SafePrimesKeyVerParameters param);

        Task<KasSscAftResult> GetKasSscAftTestAsync(KasSscAftParameters param);
        Task<KasSscAftDeferredResult> CompleteDeferredKasSscAftTestAsync(KasSscAftDeferredParameters param);
        Task<KasSscValResult> GetKasSscValTestAsync(KasSscValParameters param, bool firstAttempt);

        Task<KasSscAftResultIfc> GetKasIfcSscAftTestAsync(KasSscAftParametersIfc param);
        Task<KasSscAftDeferredResultIfc> CompleteDeferredKasIfcSscAftTestAsync(KasSscAftDeferredParametersIfc param);
        Task<KasSscValResultIfc> GetKasIfcSscValTestAsync(KasSscValParametersIfc param, bool firstAttempt);

        Task<KasKcAftResult> GetKasKcAftTestAsync(KasKcAftParameters param);

        Task<KdaAftOneStepResult> GetKdaAftOneStepTestAsync(KdaAftOneStepParameters param);
        Task<KdaValOneStepResult> GetKdaValOneStepTestAsync(KdaValOneStepParameters param);

        Task<KdaAftOneStepNoCounterResult> GetKdaAftOneStepNoCounterTestAsync(KdaAftOneStepNoCounterParameters param);
        Task<KdaValOneStepNoCounterResult> GetKdaValOneStepNoCounterTestAsync(KdaValOneStepNoCounterParameters param);

        Task<KdaAftTwoStepResult> GetKdaAftTwoStepTestAsync(KdaAftTwoStepParameters param);
        Task<KdaValTwoStepResult> GetKdaValTwoStepTestAsync(KdaValTwoStepParameters param);

        Task<KdaAftHkdfResult> GetKdaAftHkdfTestAsync(KdaAftHkdfParameters param);
        Task<KdaValHkdfResult> GetKdaValHkdfTestAsync(KdaValHkdfParameters param);

        Task<DsaKeyResult> GetSafePrimeKeyAsync(SafePrimesKeyGenParameters param);
    }
}
