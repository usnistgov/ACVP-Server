using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly AeadModeBlockCipherFactory _aeadModeBlockCipherFactory = new AeadModeBlockCipherFactory();

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

        private AeadResult GetAesCcmCase(AeadParameters param)
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Iv = _rand.GetRandomBitString(param.IvLength),
                Aad = _rand.GetRandomBitString(param.AadLength)
            };

            var result = DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Ccm
                ), 
                fullParams, param
            );

            if (param.CouldFail)
            {
                // Should Fail at certain ratio, 25%
                var upperBound = (int)(1.0 / GCM_FAIL_RATIO);
                var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

                if (shouldFail)
                {
                    var ctPortion = param.DataLength == 0
                        ? new BitString(0)
                        : result.CipherText.GetMostSignificantBits(result.CipherText.BitLength - param.TagLength);
                    // Change the tag portion of the ciphertext
                    var tagPortion = _rand.GetDifferentBitStringOfSameSize(result.CipherText.GetLeastSignificantBits(param.TagLength));

                    result.CipherText = ctPortion.ConcatenateBits(tagPortion);
                    result.TestPassed = false;
                }
            }

            return result;
        }

        private AeadResult GetEcmaCase(AeadParameters param)
        {
            var flags = new BitString("59");
            var nonce = _rand.GetRandomBitString(13 * 8);
            var aadLen = MsbLsbConversionHelpers.ReverseByteOrder(BitString.To16BitString((short) ((param.AadLength - (14 * 8)) / 8)).ToBytes());
            var dataLen = BitString.To16BitString((short) (param.DataLength / 8)).ToBytes();

            var aad = new BitString("581C")
                .ConcatenateBits(nonce.Substring(2 * 8, 2 * 8))
                .ConcatenateBits(nonce.Substring(0, 2 * 8))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(new BitString(aadLen))
                .ConcatenateBits(BitString.Zeroes(2 * 8))
                .ConcatenateBits(_rand.GetRandomBitString(param.AadLength - (14  * 8)));

            var iv = flags
                .ConcatenateBits(nonce)
                .ConcatenateBits(new BitString(dataLen));

            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Iv = nonce,
                Aad = aad
            };

            var returnResult = DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes),
                    BlockCipherModesOfOperation.Ccm
                ),
                fullParams, param
            );

            returnResult.Iv = iv;
            return returnResult;
        }

        private AeadResult GetAesGcmCase(AeadParameters param)
        {
            var fullParams = new AeadResult
            {
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength),
                Iv = _rand.GetRandomBitString(param.IvLength),
                Aad = _rand.GetRandomBitString(param.AadLength)
            };

            var result = DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                fullParams, 
                param
            );

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

        private AeadResult GetAesXpnCase(AeadParameters param)
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
            var result = DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                tempParams, 
                param
            );

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

        private AeadResult GetDeferredAesGcmCase(AeadParameters param)
        {
            return new AeadResult
            {
                Aad = _rand.GetRandomBitString(param.AadLength),
                PlainText = _rand.GetRandomBitString(param.DataLength),
                Key = _rand.GetRandomBitString(param.KeyLength)
            };
        }

        private AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam)
        {
            return DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                fullParam, 
                param
            );
        }

        private AeadResult GetDeferredAesXpnCase(AeadParameters param)
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

        private AeadResult CompleteDeferredAesXpnCase(AeadParameters param, AeadResult fullParam)
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

            return DoSimpleAead(
                _aeadModeBlockCipherFactory.GetAeadCipher(
                    _engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Aes), 
                    BlockCipherModesOfOperation.Gcm
                ), 
                testParam, 
                param
            );
        }

        public async Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetAesCcmCase(param));
        }

        public async Task<AeadResult> GetEcmaCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetEcmaCase(param));
        }

        public async Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetAesGcmCase(param));
        }

        public async Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetAesXpnCase(param));
        }

        public async Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredAesGcmCase(param));
        }

        public async Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredAesGcmCase(param, fullParam));
        }

        public async Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            return await _taskFactory.StartNew(() => GetDeferredAesXpnCase(param));
        }

        public async Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            return await _taskFactory.StartNew(() => CompleteDeferredAesXpnCase(param, fullParam));
        }
    }
}
