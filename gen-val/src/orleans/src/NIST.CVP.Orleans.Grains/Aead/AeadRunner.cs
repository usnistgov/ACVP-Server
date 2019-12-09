using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.Orleans.Grains.Aead
{
    public class AeadRunner : IAeadRunner
    {
        public AeadResult DoSimpleAead(IAeadModeBlockCipher cipher, AeadResult fullParam, AeadParameters param)
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
    }
}