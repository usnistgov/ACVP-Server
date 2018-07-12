using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using System;
using System.Linq;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly AesEngine _aes = new AesEngine();
        private readonly ModeBlockCipherFactory _modeFactory = new ModeBlockCipherFactory();
        private readonly AesMonteCarloFactory _aesMctFactory = new AesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly CounterFactory _ctrFactory = new CounterFactory();

        public AesResult GetAesCase(AesParameters param)
        {
            var cipher = _modeFactory.GetStandardCipher(_aes, param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new AesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            };
        }

        public MctResult<AesResult> GetAesMctCase(AesParameters param)
        {
            var cipher = _aesMctFactory.GetInstance(param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength);
            var iv = _rand.GetRandomBitString(128);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new MctResult<AesResult>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new AesResult
                {
                    Key = element.Key,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }

        public AesResult GetDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            var iv = GetStartingIV(param.Overflow, param.Incremental);

            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.Parameters.DataLength);
            var key = _rand.GetRandomBitString(param.Parameters.KeyLength);

            return new AesResult
            {
                Key = key,
                Iv = iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : null,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : null
            };
        }

        public AesResult CompleteDeferredAesCounterCase(CounterParameters<AesParameters> param)
        {
            var fullParam = GetDeferredAesCounterCase(param);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Parameters.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var counter = _ctrFactory.GetCounter(_aes, param.Incremental ? CounterTypes.Additive : CounterTypes.Subtractive, fullParam.Iv);
            var cipher = _modeFactory.GetCounterCipher(_aes, counter);

            var blockCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText, null);
  
            var result = cipher.ProcessPayload(blockCipherParams);

            return new AesResult
            {
                Key = fullParam.Key,
                Iv = fullParam.Iv,
                PlainText = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? fullParam.CipherText : result.Result
            };
        }

        public CounterResult ExtractIvs(AesParameters param, AesResult fullParam)
        {
            var cipher = _modeFactory.GetIvExtractor(_aes);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = direction == BlockCipherDirections.Encrypt ? fullParam.PlainText : fullParam.CipherText;
            var result = direction == BlockCipherDirections.Encrypt ? fullParam.CipherText : fullParam.PlainText;

            var counterCipherParams = new CounterModeBlockCipherParameters(direction, fullParam.Iv, fullParam.Key, payload, result);

            var extractedIvs = cipher.ExtractIvs(counterCipherParams);

            if (!extractedIvs.Success)
            {
                // TODO log error somewhere
                throw new Exception();
            }

            return new CounterResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? null : extractedIvs.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? null : extractedIvs.Result,
                IVs = extractedIvs.IVs
            };
        }

        public AesXtsResult GetAesXtsCase(AesXtsParameters param)
        {
            var cipher = _modeFactory.GetStandardCipher(_aes, param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = _rand.GetRandomBitString(param.KeyLength * 2);
            var i = new BitString(0);
            var number = 0;

            if (param.TweakMode.Equals("hex", StringComparison.OrdinalIgnoreCase))
            {
                i = _rand.GetRandomBitString(128);
            }
            else if (param.TweakMode.Equals("number", StringComparison.OrdinalIgnoreCase))
            {
                number = _rand.GetRandomInt(0, 256);
                i = XtsHelper.GetIFromInteger(number);
            }

            var blockCipherParams = new ModeBlockCipherParameters(direction, i, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            return new AesXtsResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                SequenceNumber = number,
                Iv = i,
                Key = key
            };
        }

        private BitString GetStartingIV(bool overflow, bool incremental)
        {
            BitString padding;

            // Arbitrary 'small' value so samples and normal registrations always hit boundary
            //int randomBits = _isSample ? 6 : 9;
            int randomBits = 6;

            if (overflow == incremental)
            {
                padding = BitString.Ones(128 - randomBits);
            }
            else
            {
                padding = BitString.Zeroes(128 - randomBits);
            }

            return BitString.ConcatenateBits(padding, _rand.GetRandomBitString(randomBits));
        }
    }
}
