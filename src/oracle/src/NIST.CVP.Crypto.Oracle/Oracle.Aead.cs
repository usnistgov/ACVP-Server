using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.AES_GCM;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private AeadResult DoSimpleAead(IAeadModeBlockCipher cipher, AeadParameters param)
        {
            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(param.IvLength);
            var aad = _rand.GetRandomBitString(param.AadLength);
            var tagLength = param.TagLength;

            var aeadBlockCipherParams = new AeadModeBlockCipherParameters(BlockCipherDirections.Encrypt, iv, key, payload, aad, tagLength);
            var result = cipher.ProcessPayload(aeadBlockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new AeadResult
            {
                PlainText = payload,
                Key = key,
                Iv = iv,
                CipherText = result.Result,
                Tag = result.Tag,
                TestPassed = true
            };
        }

        public AeadResult GetAesCcmCase() => throw new NotImplementedException();

        public AeadResult GetAesGcmCase(AeadParameters param)
        {
            var cipher = new GcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));
            var result = DoSimpleAead(cipher, param);

            // Should Fail at certain ratio, 25%
            var upperBound = (int)(1.0 / GCM_FAIL_RATIO);
            var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

            if (shouldFail)
            {
                result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                result.TestPassed = false;
            }

            return result;
        }

        public AeadResult GetAesXpnCase(AeadParameters param)
        {
            var cipher = new GcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_GCMInternals(new ModeBlockCipherFactory(), new BlockCipherEngineFactory()));
            var result = DoSimpleAead(cipher, param);

            // Should Fail at certain ratio, 25%
            var upperBound = (int)(1.0 / XPN_FAIL_RATIO);
            var shouldFail = _rand.GetRandomInt(0, upperBound) == 0;

            if (shouldFail)
            {
                result.Tag = _rand.GetDifferentBitStringOfSameSize(result.Tag);
                result.TestPassed = false;
            }

            return result;
        }
    }
}
