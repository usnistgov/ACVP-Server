using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly PQGeneratorValidatorFactory _pqGenFactory = new PQGeneratorValidatorFactory();
        private readonly GGeneratorValidatorFactory _gGenFactory = new GGeneratorValidatorFactory();

        public DsaDomainParametersResult GetDsaPQ(DsaDomainParametersParameters param)
        {
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

        public DsaDomainParametersResult GetDsaG(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
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

        public DsaDomainParametersResult GetDsaDomainParameters(DsaDomainParametersParameters param)
        {
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

        public VerifyResult<DsaDomainParametersResult> GetDsaPQVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
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

        public VerifyResult<DsaDomainParametersResult> GetDsaGVerify(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
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

        public DsaDomainParametersResult CompleteDeferredDsaDomainParameters(DsaDomainParametersParameters param, DsaKeyResult fullParam)
        {
            throw new NotImplementedException();
        }

        public VerifyResult<DsaDomainParametersResult> GetDsaDomainParametersVerify(DsaDomainParametersParameters param)
        {
            throw new NotImplementedException();
        }

        public DsaKeyResult GetDsaKey(DsaKeyParameters param)
        {
            throw new NotImplementedException();
        }

        public DsaKeyResult CompleteDeferredDsaKey(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            throw new NotImplementedException();
        }

        public VerifyResult<DsaKeyResult> GetDsaKeyVerify(DsaKeyParameters param)
        {
            throw new NotImplementedException();
        }

        public DsaSignatureResult GetDeferredDsaSignature(DsaSignatureParameters param)
        {
            throw new NotImplementedException();
        }

        public VerifyResult<DsaSignatureResult> CompleteDeferredDsaSignature(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            throw new NotImplementedException();
        }

        public DsaSignatureResult GetDsaSignature(DsaSignatureParameters param)
        {
            throw new NotImplementedException();
        }

        public VerifyResult<DsaSignatureResult> GetDsaVerifyResult(DsaSignatureParameters param)
        {
            throw new NotImplementedException();
        }
    }
}
