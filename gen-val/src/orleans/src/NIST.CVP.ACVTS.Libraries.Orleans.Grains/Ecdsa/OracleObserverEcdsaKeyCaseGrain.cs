using System;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaKeyCaseCaseGrain : ObservableOracleGrainBase<EcdsaKeyResult>,
        IOracleObserverEcdsaKeyCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;

        private EcdsaKeyParameters _param;

        public OracleObserverEcdsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
        }

        public async Task<bool> BeginWorkAsync(EcdsaKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);
            var entropyProvider = new EntropyProvider(new Random800_90());
            IDsaEcc eccDsa;

            // For deterministic ECDSA signature generation, the per-message secret number is a function of the private key, d. 
            // As part of calculating the per-message secret number, d, a non-negative integer in the interval [1, n-1], is converted
            // to an octet string rlen = 8 * ceil(len(n)/8) bits in length. Depending on the value of d, its octet string representation 
            // needs to be left-padded with 0-bits until it is rlen bits in length. Creating a keypair that uses a small value
            // for d will provide a test that ensures IUTs are correctly left-padding the octet string representation of d
            // to rlen bits. Correctly left-padding the octet string representation of d has direct bearing on whether an implementation
            // is calculating the per-message secret number and beyond that, the message signature, correctly.
            if (_param.GenerateKeyPairWithSmallRandomD)
            {
                // The number of bits needed to represent n
                var nLen = domainParams.CurveE.OrderN.ExactBitLength();
                
                // d is defined to be a non-negative integer in the interval [1, n-1]. From this we can infer that the number of
                // bits needed to represent a valid d will always be <= nLen.  
                // Generate a random number d such that d can be represented in nlen/4 bits.
                // The largest number that can be represented in nlen/4 bits will be 2^(nlen/4) - 1.
                // Generate a small random number d [1, 2^(nlen/4) - 1]. With a value for d this small, the IUT will need to left-pad
                // at least 3*nlen/4 0-bits to the octet representation of d when calculating the per-message secret number.
                var d = entropyProvider.GetEntropy(1, BigInteger.Pow(2, nLen/4) - 1);
                
                // Using a TestableEntropyProvider when we ask the factory to give us our EccDsa instance allows us to pass through
                // to the EccDsa instance the value we want to be used for d. We load the value we want to be used for d into the
                // TestableEntropyProvider. When EccDsa.GenerateKeyPair() requests a "random" value to us for d, the TestableEntropyProvider
                // will pass back the value we loaded into it for this purpose.
                var testableEntropyProvider = new TestableEntropyProvider();
                testableEntropyProvider.AddEntropy(d);
                eccDsa = _dsaFactory.GetInstanceForKeys(testableEntropyProvider);
            }
            else
            {
                eccDsa = _dsaFactory.GetInstanceForKeys(entropyProvider);
            }
            
            var result = eccDsa.GenerateKeyPair(domainParams);
            
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EcdsaKeyResult
            {
                Key = result.KeyPair
            });
        }
    }
}
