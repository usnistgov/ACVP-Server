using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;
using System;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly GcmBlockCipher _gcm = new GcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));
        private readonly CcmBlockCipher _ccm = new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals());

        private AeadResult DoSimpleAead(IAeadModeBlockCipher cipher, AeadResult fullParam, AeadParameters param)
        {
            var aeadBlockCipherParams = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, fullParam.Iv, fullParam.Key, fullParam.PlainText, fullParam.Aad, param.TagLength);
            var result = cipher.ProcessPayload(aeadBlockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new AeadResult
            {
                PlainText = fullParam.PlainText,
                Key = fullParam.Key,
                Iv = fullParam.Iv,
                Aad = fullParam.Aad,
                CipherText = result.Result,
                Tag = result.Tag,
                TestPassed = true
            };
        }

        public AeadResult GetAesCcmCase(AeadParameters param)
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Iv = _rand.GetRandomBitString(param.IvLength),
                Aad = _rand.GetRandomBitString(param.AadLength),
            };

            return DoSimpleAead(_ccm, fullParams, param);
        }

        public AeadResult GetAesGcmCase(AeadParameters param)
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Iv = _rand.GetRandomBitString(param.IvLength),
                Aad = _rand.GetRandomBitString(param.AadLength)
            };

            var result = DoSimpleAead(_gcm, fullParams, param);

            if (param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / GCM_FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                    result.TestPassed = false;
                }
            }

            return result;
        }

        public AeadResult GetAesXpnCase(AeadParameters param)
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Salt = _rand.GetRandomBitString(param.SaltLength),
                Iv = _rand.GetRandomBitString(param.IvLength),
                Aad = _rand.GetRandomBitString(param.AadLength)
            };

            var tempParams = new AeadResult
            {
                PlainText = fullParams.PlainText,
                Key = fullParams.Key,
                Iv = fullParams.Iv.XOR(fullParams.Salt),
                Aad = fullParams.Aad
            };

            // Uses gcm as a cipher instead of xpn
            var result = DoSimpleAead(_gcm, tempParams, param);

            // Should Fail at certain ratio, 25%
            var upperBound = (int)(1.0 / XPN_FAIL_RATIO);
            var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

            if (shouldFail)
            {
                result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                result.TestPassed = false;
            }

            result.Iv = fullParams.Iv;
            result.Salt = fullParams.Salt;

            return result;
        }

        public AeadResult GetDeferredAesGcmCase(AeadParameters param)
        {
            return new AeadResult
            {
                Aad = _rand.GetRandomBitString(param.AadLength),
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength)
            };
        }

        public AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam)
        {
            return DoSimpleAead(_gcm, fullParam, param);
        }

        public AeadResult GetDeferredAesXpnCase(AeadParameters param)
        {
            return new AeadResult
            {
                Aad = _rand.GetRandomBitString(param.AadLength),
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Salt = param.ExternalSalt ? _rand.GetRandomBitString(param.SaltLength) : null,
                Iv = param.ExternalIv ? _rand.GetRandomBitString(param.IvLength) : null
            };
        }

        public AeadResult CompleteDeferredAesXpnCase(AeadParameters param, AeadResult fullParam)
        {
            var testParam = new AeadResult
            {
                Aad = fullParam.Aad,
                CipherText = fullParam.CipherText,
                Iv = fullParam.Iv.XOR(fullParam.Salt),
                Key = fullParam.Key,
                PlainText = fullParam.PlainText,
                Tag = fullParam.Tag
            };

            return DoSimpleAead(_gcm, testParam, param);
        }
    }
}
