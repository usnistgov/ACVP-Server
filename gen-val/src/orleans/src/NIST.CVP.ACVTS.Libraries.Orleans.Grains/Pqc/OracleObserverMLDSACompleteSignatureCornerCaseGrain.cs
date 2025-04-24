using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Dilithium;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverMLDSACompleteSignatureCornerCaseGrain : ObservableOracleGrainBase<MLDSASignatureResult>, IOracleObserverMLDSACompleteSignatureCornerCaseGrain
{
    private MLDSASignatureParameters _param;
    private MLDSASignatureResult _poolResult;

    private readonly IShaFactory _shaFactory;
    
    public OracleObserverMLDSACompleteSignatureCornerCaseGrain(        
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _shaFactory = shaFactory;
    }

    public async Task<bool> BeginWorkAsync(MLDSASignatureParameters param, MLDSASignatureResult poolResult)
    {
        _param = param;
        _poolResult = poolResult;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var mldsa = new Dilithium(_param.ParameterSet, _shaFactory);
        
        var key = mldsa.GenerateKey(_poolResult.Seed.Bits);
        var signature = mldsa.Sign(key.sk, _poolResult.Message.ToBytes(), BitString.Zeroes(256).ToBytes());
        
        var result = new MLDSASignatureResult
        {
            Message = _poolResult.Message,
            PrivateKey = new BitString(key.sk),
            Signature = new BitString(signature)
        };

        await Notify(result);
    }
}
