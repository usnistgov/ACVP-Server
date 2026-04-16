using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.SPDM;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SPDM;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.SPDM;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kdf;

public class OracleObserverSPDMCaseGrain : ObservableOracleGrainBase<SPDMResult>, IOracleObserverSPDMCaseGrain
{
    private SPDMParameters _param;
    private readonly IRandom800_90 _rand;

    public OracleObserverSPDMCaseGrain(LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler, IRandom800_90 rand) : base(nonOrleansScheduler)
    {
        _rand = rand;
    }

    public async Task<bool> BeginWorkAsync(SPDMParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var key = _rand.GetRandomBitString(_param.KeyLength);
        var TH1 = _rand.GetRandomBitString(_param.THLength);
        var TH2 = _rand.GetRandomBitString(_param.THLength);

        var spdm = new Spdm(ShaAttributes.GetHashFunctionFromEnum(_param.Mode));
        var spdmreturn = spdm.KeySchedule(key, _param.PSK, _param.Version, TH1, TH2);

        var result = new SPDMResult()
        {
            RequestDirectionHandshake = spdmreturn.RequestDirectionHandshake,
            ResponseDirectionHandshake = spdmreturn.ResponseDirectionHandshake,
            RequestDirectionData = spdmreturn.RequestDirectionData,
            ResponseDirectionData = spdmreturn.ResponseDirectionData,
            ExportMaster = spdmreturn.ExportMaster,
            Key = key,
            TH1 = TH1,
            TH2 = TH2,
        };

        await Notify(result);
    }
}
