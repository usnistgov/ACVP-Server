using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.Ed;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly EdwardsCurveFactory _edCurveFactory = new EdwardsCurveFactory();
        private readonly DsaEdFactory _edFactory = new DsaEdFactory();

        private EddsaKeyResult GetEddsaKey(EddsaKeyParameters param)
        {
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var domainParams = new EdDomainParameters(curve, new ShaFactory());

            // Hash function is not used, but the factory requires it
            var edDsa = _edFactory.GetInstance(null);

            var result = edDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EddsaKeyResult
            {
                Key = result.KeyPair
            };
        }

        private EddsaKeyResult CompleteDeferredEddsaKey(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var domainParams = new EdDomainParameters(curve, new ShaFactory());

            var edDsa = _edFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256), EntropyProviderTypes.Testable);
            edDsa.AddEntropy(fullParam.Key.PrivateD);

            var result = edDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EddsaKeyResult
            {
                Key = result.KeyPair
            };
        }

        private VerifyResult<EddsaKeyResult> GetEddsaKeyVerify(EddsaKeyParameters param)
        {
            var key = GetEddsaKey(param).Key;
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var edParam = new EdDomainParameters(curve, new ShaFactory());

            if (param.Disposition == EddsaKeyDisposition.NotOnCurve)
            {
                // Modify the public key value until the point is no longer on the curve
                var modifiedPublicQ = curve.Decode(key.PublicQ);

                do
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X + 1, modifiedPublicQ.Y);
                } while (curve.PointExistsOnCurve(modifiedPublicQ));

                key = new EdKeyPair(curve.Encode(modifiedPublicQ), key.PrivateD);
            }
            else if (param.Disposition == EddsaKeyDisposition.OutOfRange)
            {
                // Make Qx or Qy out of range by adding the field size
                var modifiedPublicQ = curve.Decode(key.PublicQ);

                // Get a random number 0, or 1
                if (_rand.GetRandomInt(0, 2) == 0)
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X + curve.FieldSizeQ, modifiedPublicQ.Y);
                }
                else
                {
                    modifiedPublicQ = new EdPoint(modifiedPublicQ.X, modifiedPublicQ.Y + curve.FieldSizeQ);
                }

                key = new EdKeyPair(curve.Encode(modifiedPublicQ), key.PrivateD);
            }

            return new VerifyResult<EddsaKeyResult>
            {
                Result = param.Disposition == EddsaKeyDisposition.None,
                VerifiedValue = new EddsaKeyResult
                {
                    Key = key
                }
            };
        }

        private EddsaSignatureResult GetEddsaSignature(EddsaSignatureParameters param)
        {
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var noContext = param.Curve == Common.Asymmetric.DSA.Ed.Enums.Curve.Ed25519 && !param.PreHash;
            var domainParams = new EdDomainParameters(curve, new ShaFactory());
            var edDsa = _edFactory.GetInstance(null);

            var message = _rand.GetRandomBitString(1024);

            BitString context;
            if (noContext)
            {
                context = new BitString("");
            }
            else
            {
                context = _rand.GetRandomBitString(_rand.GetRandomInt(0, 255) * 8);
            }

            var result = edDsa.Sign(domainParams, param.Key, message, context, param.PreHash);
            if (!result.Success)
            {
                throw new Exception();
            }

            return new EddsaSignatureResult
            {
                Message = message,
                Context = context,
                Signature = result.Signature
            };
        }

        private EddsaSignatureResult GetDeferredEddsaSignature(EddsaSignatureParameters param)
        {
            var message = _rand.GetRandomBitString(1024);

            var context = _rand.GetRandomBitString(_rand.GetRandomInt(0, 255) * 8);

            return new EddsaSignatureResult
            {
                Message = message,
                Context = context
            };
        }

        private VerifyResult<EddsaSignatureResult> CompleteDeferredEddsaSignature(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            var edDsa = _edFactory.GetInstance(null);
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var domainParams = new EdDomainParameters(curve, new ShaFactory());

            var result = edDsa.Verify(domainParams, param.Key, fullParam.Message, fullParam.Signature, fullParam.Context , param.PreHash);

            return new VerifyResult<EddsaSignatureResult>
            {
                Result = result.Success
            };
        }

        private VerifyResult<EddsaSignatureResult> GetEddsaVerifyResult(EddsaSignatureParameters param)
        {
            var keyParam = new EddsaKeyParameters
            {
                Curve = param.Curve
            };
            var key = GetEddsaKey(keyParam).Key;
            var curve = _edCurveFactory.GetCurve(param.Curve);
            var domainParams = new EdDomainParameters(curve, new ShaFactory());
            var edDsa = _edFactory.GetInstance(null);

            var message = _rand.GetRandomBitString(1024);

            var result = edDsa.Sign(domainParams, key, message);
            if (!result.Success)
            {
                throw new Exception();
            }

            var sigResult = new EddsaSignatureResult
            {
                Message = message,
                Key = key,
                Signature = result.Signature
            };

            if (param.Disposition == EddsaSignatureDisposition.ModifyMessage)
            {
                // Generate a different random message
                sigResult.Message = _rand.GetDifferentBitStringOfSameSize(message);
            }
            else if (param.Disposition == EddsaSignatureDisposition.ModifyKey)
            {
                // Generate a different key pair for the test case
                var keyResult = GetEddsaKey(keyParam).Key;
                sigResult.Key = keyResult;
            }
            else if (param.Disposition == EddsaSignatureDisposition.ModifyR)
            {
                //var modifiedRSignature = new EdSignature(sigResult.Signature.Sig + 1, sigResult.Signature.S);
                //sigResult.Signature = modifiedRSignature;
            }
            else if (param.Disposition == EddsaSignatureDisposition.ModifyS)
            {
                //var modifiedSSignature = new edSignature(sigResult.Signature.R, sigResult.Signature.S + 1);
                //sigResult.Signature = modifiedSSignature;
            }

            return new VerifyResult<EddsaSignatureResult>
            {
                Result = param.Disposition == EddsaSignatureDisposition.None,
                VerifiedValue = sigResult
            };
        }

        public async Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetEddsaKey(param));
        }

        public async Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredEddsaKey(param, fullParam));
        }

        public async Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param)
        {
            return await _taskFactory.StartNew(() => GetEddsaKeyVerify(param));
        }

        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredEddsaSignature(param));
        }

        public async Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredEddsaSignature(param, fullParam));
        }

        public async Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetEddsaSignature(param));
        }

        public async Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param)
        {
            return await _taskFactory.StartNew(() => GetEddsaVerifyResult(param));
        }
    }
}
