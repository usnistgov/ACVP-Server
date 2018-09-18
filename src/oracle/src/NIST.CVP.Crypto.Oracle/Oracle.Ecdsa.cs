using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly EccCurveFactory _curveFactory = new EccCurveFactory();
        private readonly DsaEccFactory _eccFactory = new DsaEccFactory(new ShaFactory());

        private EcdsaKeyResult GetEcdsaKey(EcdsaKeyParameters param)
        {
            var poolBoy = new PoolBoy<EcdsaKeyResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.ECDSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);

            // Hash function is not used, but the factory requires it
            var eccDsa = _eccFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

            var result = eccDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EcdsaKeyResult
            {
                Key = result.KeyPair
            };
        }

        private EcdsaKeyResult CompleteDeferredEcdsaKey(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);

            var eccDsa = _eccFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256), EntropyProviderTypes.Testable);
            eccDsa.AddEntropy(fullParam.Key.PrivateD);

            var result = eccDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EcdsaKeyResult
            {
                Key = result.KeyPair
            };
        }

        private VerifyResult<EcdsaKeyResult> GetEcdsaKeyVerify(EcdsaKeyParameters param)
        {
            var key = GetEcdsaKey(param).Key;
            var curve = _curveFactory.GetCurve(param.Curve);

            if (param.Disposition == EcdsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = key.PublicQ;

                do
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                key = new EccKeyPair(modifiedPublicQ, key.PrivateD);
            }
            else if (param.Disposition == EcdsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = key.PublicQ;

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EccPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                key = new EccKeyPair(modifiedPublicQ, key.PrivateD);
            }

            return new VerifyResult<EcdsaKeyResult>
            {
                Result = param.Disposition == EcdsaKeyDisposition.None,
                VerifiedValue = new EcdsaKeyResult
                {
                    Key = key
                }
            };
        }

        private EcdsaSignatureResult GetEcdsaSignature(EcdsaSignatureParameters param)
        {
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);
            var eccDsa = _eccFactory.GetInstance(param.HashAlg);

            var message = _rand.GetRandomBitString(param.PreHashedMessage ? param.HashAlg.OutputLen : 1024);

            var result = eccDsa.Sign(domainParams, param.Key, message, param.PreHashedMessage);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EcdsaSignatureResult
            {
                Message = message,
                Signature = result.Signature
            };
        }

        private EcdsaSignatureResult GetDeferredEcdsaSignature(EcdsaSignatureParameters param)
        {
            var message = _rand.GetRandomBitString(param.PreHashedMessage ? param.HashAlg.OutputLen : 1024);

            return new EcdsaSignatureResult
            {
                Message = message
            };
        }

        private VerifyResult<EcdsaSignatureResult> CompleteDeferredEcdsaSignature(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            var eccDsa = _eccFactory.GetInstance(param.HashAlg);
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);

            var result = eccDsa.Verify(domainParams, param.Key, fullParam.Message, fullParam.Signature, param.PreHashedMessage);

            return new VerifyResult<EcdsaSignatureResult>
            {
                Result = result.Success
            };
        }

        private VerifyResult<EcdsaSignatureResult> GetEcdsaVerifyResult(EcdsaSignatureParameters param)
        {
            var keyParam = new EcdsaKeyParameters
            {
                Curve = param.Curve
            };
            var key = GetEcdsaKey(keyParam).Key;
            var curve = _curveFactory.GetCurve(param.Curve);
            var domainParams = new EccDomainParameters(curve);
            var eccDsa = _eccFactory.GetInstance(param.HashAlg);

            var message = _rand.GetRandomBitString(1024);

            var result = eccDsa.Sign(domainParams, key, message);
            if (!result.Success)
            {
                throw new Exception();
            }

            var sigResult = new EcdsaSignatureResult
            {
                Message = message,
                Key = key,
                Signature = result.Signature
            };

            if (param.Disposition == EcdsaSignatureDisposition.ModifyMessage)
            {
                // Generate a different random message
                sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            else if (param.Disposition == EcdsaSignatureDisposition.ModifyKey)
            {
                // Generate a different key pair for the test case
                var keyResult = GetEcdsaKey(keyParam).Key;
                sigResult.Key = keyResult;
            }
            else if (param.Disposition == EcdsaSignatureDisposition.ModifyR)
            {
                var modifiedRSignature = new EccSignature(sigResult.Signature.R + 1, sigResult.Signature.S);
                sigResult.Signature = modifiedRSignature;
            }
            else if (param.Disposition == EcdsaSignatureDisposition.ModifyS)
            {
                var modifiedSSignature = new EccSignature(sigResult.Signature.R, sigResult.Signature.S + 1);
                sigResult.Signature = modifiedSSignature;
            }

            return new VerifyResult<EcdsaSignatureResult>
            {
                Result = param.Disposition == EcdsaSignatureDisposition.None,
                VerifiedValue = sigResult
            };
        }

        public async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetEcdsaKey(param));
        }

        public async Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredEcdsaKey(param, fullParam));
        }

        public async Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetEcdsaKeyVerify(param));
        }

        public async Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredEcdsaSignature(param));
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredEcdsaSignature(param, fullParam));
        }

        public async Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetEcdsaSignature(param));
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetEcdsaVerifyResult(param));
        }
    }
}
