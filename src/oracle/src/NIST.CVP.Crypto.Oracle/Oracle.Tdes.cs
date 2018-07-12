using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using System;
using System.Linq;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly TdesEngine _tdes = new TdesEngine();
        private readonly TdesMonteCarloFactory _tdesMctFactory = new TdesMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        private readonly TdesPartitionsMonteCarloFactory _tdesWithIvsMctFactory = new TdesPartitionsMonteCarloFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());

        public TdesResult GetTdesCase(TdesParameters param)
        {
            var cipher = _modeFactory.GetStandardCipher(_tdes, param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new TdesResult
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv = iv
            };
        }

        public MctResult<TdesResult> GetTdesMctCase(TdesParameters param)
        {
            var cipher = _tdesMctFactory.GetInstance(param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new MctResult<TdesResult>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResult
                {
                    Key = element.Keys,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }

        public TdesResultWithIvs GetTdesWithIvsCase(TdesParameters param)
        {
            var cipher = _modeFactory.GetStandardCipher(_tdes, param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);
            var ivs = TdesPartitionHelpers.SetupIvs(iv);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessPayload(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new TdesResultWithIvs
            {
                PlainText = direction == BlockCipherDirections.Encrypt ? payload : result.Result,
                CipherText = direction == BlockCipherDirections.Decrypt ? payload : result.Result,
                Key = key,
                Iv1 = ivs[0],
                Iv2 = ivs[1],
                Iv3 = ivs[2]
            };
        }

        public MctResult<TdesResultWithIvs> GetTdesMctWithIvsCase(TdesParameters param)
        {
            var cipher = _tdesWithIvsMctFactory.GetInstance(param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            return new MctResult<TdesResultWithIvs>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResultWithIvs
                {
                    Key = element.Keys,
                    Iv1 = element.IV1,
                    Iv2 = element.IV2,
                    Iv3 = element.IV3,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }
    }
}
