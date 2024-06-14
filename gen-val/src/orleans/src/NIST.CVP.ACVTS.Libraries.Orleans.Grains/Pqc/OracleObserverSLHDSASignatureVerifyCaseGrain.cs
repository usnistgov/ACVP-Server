using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.SLHDSA;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Pqc;

public class OracleObserverSLHDSASignatureVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<SLHDSASignatureResult>>, IOracleObserverSLHDSASignatureVerifyCaseGrain
{
    private readonly IEntropyProvider _entropyProvider;
    private readonly IShaFactory _shaFactory;

    private SLHDSASignatureParameters _param;

    public OracleObserverSLHDSASignatureVerifyCaseGrain(
        LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
        IEntropyProviderFactory entropyProviderFactory,
        IShaFactory shaFactory
    ) : base(nonOrleansScheduler)
    {
        _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        _shaFactory = shaFactory;
    }
    
    public async Task<bool> BeginWorkAsync(SLHDSASignatureParameters param)
    {
        _param = param;

        await BeginGrainWorkAsync();
        return await Task.FromResult(true);
    }

    protected override async Task DoWorkAsync()
    {
        var wots = new Wots(_shaFactory);
        var xmss = new Xmss(_shaFactory, wots);
        var hypertree = new Hypertree(xmss);
        var fors = new Fors(_shaFactory);
        var slhdsa = new Slhdsa(_shaFactory, xmss, hypertree, fors);
        
        // pull back all of the attribute values associated with the SLHDSA parameter set in use
        var slhdsaParameterSetAttributes = AttributesHelper.GetParameterSetAttribute(_param.SlhdsaParameterSet);
        
        // the message to be signed
        var message = _entropyProvider.GetEntropy(_param.MessageLength);
        
        // generate the key pair with which to 1) sign the message and 2) verify the signature 
        var SKSeed = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();
        var SKPrf = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();
        var PKSeed = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8).ToBytes();

        var keyPair = slhdsa.SlhKeyGen(SKSeed, SKPrf, PKSeed, slhdsaParameterSetAttributes);

        // The signature verification process is independent of whether a signature was signed deterministically or
        // non-deterministically. Do: use a non-deterministic signature as the standard indicates a preference for
        // non-deterministic signing.
        var additionalRandomness = _entropyProvider.GetEntropy(slhdsaParameterSetAttributes.N * 8);
        
        // Create the signature 
        var signature = slhdsa.SlhSignNonDeterministic(message.ToBytes(), keyPair.PrivateKey, additionalRandomness.ToBytes(), slhdsaParameterSetAttributes);
        
        var result = new SLHDSASignatureResult
        {
            PrivateKey = new BitString(keyPair.PrivateKey.GetBytes()),
            PublicKey = new BitString(keyPair.PublicKey.GetBytes()),
            AdditionalRandomness = additionalRandomness,
            MessageLength = _param.MessageLength,
            Message = message,
            Signature = new BitString(signature)
        };

        // Make any changes that may be required based on the test case disposition
        // SLH-DSA Signature format:
        // Randomness R - n bytes
        // SIGFORS - k(1 + a) * n bytes
        // SIGHT - (h + d * len) * n bytes
        switch (_param.Disposition)
        {
            case SLHDSASignatureDisposition.None:
                // no changes required
                break;
            
            case SLHDSASignatureDisposition.ModifyMessage:
                // Flip the last bit in the message, Bits is Lsb
                result.Message.Bits.Set(0, !result.Message.Bits.Get(0));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureR:
                // Flip the first bit in the signature, Bits is Lsb
                var leadingBit = result.Signature.Bits.Count - 1;
                result.Signature.Bits.Set(leadingBit, !result.Signature.Bits.Get(leadingBit));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureSigFors:
                // Flip the first bit in the SIGFORS component of the signature, Bits is Lsb
                var leadingSigForsBit = result.Signature.Bits.Count - 1 - (slhdsaParameterSetAttributes.N * 8);
                result.Signature.Bits.Set(leadingSigForsBit, !result.Signature.Bits.Get(leadingSigForsBit));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureSigHt:
                // Flip the last bit in the SIGHT component of the signature, Bits is Lsb
                result.Signature.Bits.Set(0, !result.Signature.Bits.Get(0));
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureTooLarge:
                // Make the signature larger by 1 byte
                var alteredSignature = result.Signature.ToHex() + "AA";
                result.Signature = new BitString(alteredSignature);
                break;
            
            case SLHDSASignatureDisposition.ModifySignatureTooSmall:
                // Make the signature smaller by 1 byte
                var signatureString = result.Signature.ToHex();
                result.Signature = new BitString(signatureString.Substring(0, signatureString.Length - 2));
                break;
        }
        
        await Notify(new VerifyResult<SLHDSASignatureResult>
        {
            Result = _param.Disposition == SLHDSASignatureDisposition.None,
            VerifiedValue = result
        });
    }
}
