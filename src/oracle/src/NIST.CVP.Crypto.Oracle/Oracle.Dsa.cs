using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly PQGeneratorValidatorFactory _pqGenFactory = new PQGeneratorValidatorFactory();
        private readonly GGeneratorValidatorFactory _gGenFactory = new GGeneratorValidatorFactory();
        private readonly DsaFfcFactory _dsaFactory = new DsaFfcFactory(new ShaFactory());

        private DsaDomainParametersResult GetDsaPQ(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                return poolResult;
            }

            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(param.PQGenMode, sha);

            var result = pqGen.Generate(param.L, param.N, param.N);
            if (!result.Success)
            {
                throw new Exception();
            }

            var domainParams = new DsaDomainParametersResult
            {
                P = result.P,
                Q = result.Q,
                Seed = result.Seed,
                Counter = result.Count
            };

            // If there's no value here, just move on
            if (param.Disposition == default(string))
            {
                return domainParams;
            }

            var friendlyReason = EnumHelpers.GetEnumFromEnumDescription<DsaPQDisposition>(param.Disposition);
            if (friendlyReason == DsaPQDisposition.ModifyP)
            {
                // Make P not prime
                do
                {
                    domainParams.P = _rand.GetRandomBitString(param.L).ToPositiveBigInteger();

                } while (NumberTheory.MillerRabin(domainParams.P, DSAHelper.GetMillerRabinIterations(param.L, param.N)));
            }
            else if (friendlyReason == DsaPQDisposition.ModifyQ)
            {
                // Modify Q so that 0 != (P-1) mod Q
                domainParams.Q = _rand.GetRandomBitString(param.N).ToPositiveBigInteger();
            }
            else if (friendlyReason == DsaPQDisposition.ModifySeed)
            {
                // Modify FirstSeed
                var oldSeed = new BitString(domainParams.Seed.Seed);
                var newSeed = _rand.GetRandomBitString(oldSeed.BitLength).ToPositiveBigInteger();

                domainParams.Seed.ModifySeed(newSeed);
            }

            return domainParams;
        }

        private DsaDomainParametersResult GetDsaG(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            // Make sure index is not "0000 0000"
            BitString index;
            do
            {
                index = _rand.GetRandomBitString(8);
            } while (index.Equals(BitString.Zeroes(8)));

            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var gGen = _gGenFactory.GetGeneratorValidator(param.GGenMode, sha);

            var result = gGen.Generate(pqParam.P, pqParam.Q, pqParam.Seed, index);
            if (!result.Success)
            {
                throw new Exception();
            }

            var domainParams = new DsaDomainParametersResult
            {
                G = result.G,
                H = result.H,
                Index = index
            };

            if (param.Disposition == default(string))
            {
                return domainParams;
            }

            // Modify g
            var friendlyReason = EnumHelpers.GetEnumFromEnumDescription<DsaGDisposition>(param.Disposition);
            if (friendlyReason == DsaGDisposition.ModifyG)
            {
                do
                {
                    domainParams.G = _rand.GetRandomBitString(param.L).ToPositiveBigInteger();

                } while (BigInteger.ModPow(domainParams.G, pqParam.Q, pqParam.P) == 1);
            }

            return domainParams;
        }

        private DsaDomainParametersResult GetDsaDomainParameters(DsaDomainParametersParameters param)
        {
            var poolBoy = new PoolBoy<DsaDomainParametersResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.DSA_PQG);
            if (poolResult != null)
            {
                return poolResult;
            }

            var pqResult = GetDsaPQ(param);
            var gResult = GetDsaG(param, pqResult);

            return new DsaDomainParametersResult
            {
                Counter = pqResult.Counter,
                G = gResult.G,
                H = gResult.H,
                Index = gResult.Index,
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed
            };
        }

        private VerifyResult<DsaDomainParametersResult> GetDsaPQVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(param.PQGenMode, sha);

            var result = pqGen.Validate(fullParam.P, fullParam.Q, fullParam.Seed, fullParam.Counter);

            return new VerifyResult<DsaDomainParametersResult>
            {
                Result = result.Success,
                VerifiedValue = fullParam
            };
        }

        private VerifyResult<DsaDomainParametersResult> GetDsaGVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var sha = _shaFactory.GetShaInstance(param.HashAlg);
            var gGen = _gGenFactory.GetGeneratorValidator(param.GGenMode, sha);

            var result = gGen.Validate(fullParam.P, fullParam.Q, fullParam.G, fullParam.Seed, fullParam.Index);

            return new VerifyResult<DsaDomainParametersResult>
            {
                Result = result.Success,
                VerifiedValue = fullParam
            };
        }

        private DsaKeyResult GetDsaKey(DsaKeyParameters param)
        {
            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            var dsa = _dsaFactory.GetInstance(hashFunction);

            var result = dsa.GenerateKeyPair(param.DomainParameters);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new DsaKeyResult
            {
                Key = result.KeyPair
            };
        }

        private VerifyResult<DsaKeyResult> CompleteDeferredDsaKey(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            var dsa = _dsaFactory.GetInstance(hashFunction);

            var result = dsa.ValidateKeyPair(param.DomainParameters, fullParam.Key);

            return new VerifyResult<DsaKeyResult>
            {
                Result = result.Success,
                VerifiedValue = fullParam
            };
        }

        private DsaSignatureResult GetDeferredDsaSignature(DsaSignatureParameters param)
        {
            return new DsaSignatureResult
            {
                Message = _rand.GetRandomBitString(param.MessageLength)
            };
        }

        private VerifyResult<DsaSignatureResult> CompleteDeferredDsaSignature(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            var ffcDsa = _dsaFactory.GetInstance(param.HashAlg);
            var verifyResult = ffcDsa.Verify(param.DomainParameters, fullParam.Key, fullParam.Message, fullParam.Signature);

            return new VerifyResult<DsaSignatureResult>
            {
                Result = verifyResult.Success,
                VerifiedValue = fullParam
            };
        }

        private DsaSignatureResult GetDsaSignature(DsaSignatureParameters param)
        {
            var message = _rand.GetRandomBitString(param.MessageLength);

            var ffcDsa = _dsaFactory.GetInstance(param.HashAlg);
            var sigResult = ffcDsa.Sign(param.DomainParameters, param.Key, message);
            if (!sigResult.Success)
            {
                throw new Exception();
            }

            var result = new DsaSignatureResult
            {
                Message = message,
                Signature = sigResult.Signature,
                Key = param.Key
            };

            if (param.Disposition == DsaSignatureDisposition.None)
            {
                return result;
            }

            // Modify message
            if (param.Disposition == DsaSignatureDisposition.ModifyMessage)
            {
                result.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            // Modify public key
            else if (param.Disposition == DsaSignatureDisposition.ModifyKey)
            {
                var x = result.Key.PrivateKeyX;
                var y = result.Key.PublicKeyY + 2;
                result.Key = new FfcKeyPair(x, y);
            }
            // Modify r
            else if (param.Disposition == DsaSignatureDisposition.ModifyR)
            {
                var s = result.Signature.S;
                var r = result.Signature.R + 2;
                result.Signature = new FfcSignature(s, r);
            }
            // Modify s
            else if (param.Disposition == DsaSignatureDisposition.ModifyS)
            {
                var s = result.Signature.S + 2;
                var r = result.Signature.R;
                result.Signature = new FfcSignature(s, r);
            }

            return result;
        }

        public async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            return await _taskFactory.StartNew(() => GetDsaPQ(param));
        }

        public async Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            return await _taskFactory.StartNew(() => GetDsaG(param, pqParam));
        }

        public async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            return await _taskFactory.StartNew(() => GetDsaDomainParameters(param));
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            return await _taskFactory.StartNew(() => GetDsaPQVerify(param, fullParam));
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            return await _taskFactory.StartNew(() => GetDsaGVerify(param, fullParam));
        }

        public async Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetDsaKey(param));
        }

        public async Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredDsaKey(param, fullParam));
        }

        public async Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredDsaSignature(param));
        }

        public async Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredDsaSignature(param, fullParam));
        }

        public async Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetDsaSignature(param));
        }
    }
}
