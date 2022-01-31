using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Aead
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
