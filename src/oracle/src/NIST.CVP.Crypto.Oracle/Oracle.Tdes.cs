using System;
using System.Linq;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Helpers;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.AlgoArrayResponse;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public TdesResult GetTdesCbcCase(TdesParameters param) => throw new NotImplementedException();
        public TdesResult GetTdesCfbCase(TdesParameters param) => throw new NotImplementedException();
        public TdesResult GetTdesEcbCase(TdesParameters param) => throw new NotImplementedException();
        public TdesResult GetTdesOfbCase(TdesParameters param) => throw new NotImplementedException();

        public TdesResultWithIvs GetTdesCbcICase(TdesParameters param) => throw new NotImplementedException();

        public TdesResultWithIvs GetTdesOfbICase(TdesParameters param)
        {
            // Check Pool first
            var cipher = new OfbiBlockCipher(new TdesEngine());
            return DoTdesWithIvs(cipher, param);
        }

        public MctResult<TdesResult> GetTdesCbcMctCase(TdesParameters param) => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesCfbMctCase(TdesParameters param) => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesEcbMctCase(TdesParameters param) => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesOfbMctCase(TdesParameters param) => throw new NotImplementedException();

        public MctResult<TdesResultWithIvs> GetTdesCbcIMctCase(TdesParameters param) => throw new NotImplementedException();

        public MctResult<TdesResultWithIvs> GetTdesOfbIMctCase(TdesParameters param)
        {
            var cipher = new MonteCarloTdesOfbi(
                new BlockCipherEngineFactory(), 
                new ModeBlockCipherFactory(),
                new TDES_OFBI.MonteCarloKeyMaker()
            );
            return DoTdesMctWithIvs(cipher, param);
        }

        private TdesResult DoSimpleTdes(IModeBlockCipher<SymmetricCipherResult> cipher, TdesParameters param)
        {
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

        private MctResult<TdesResult> DoSimpleTdesMct(IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponse>, AlgoArrayResponse> cipher, TdesParameters param)
        {
            var direction = BlockCipherDirections.Encrypt;
            if (param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(param.KeyingOption);
            var iv = _rand.GetRandomBitString(128);

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
                    Key = element.Key,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };
        }

        private TdesResultWithIvs DoTdesWithIvs(IModeBlockCipher<SymmetricCipherWithIvResult> cipher, TdesParameters param)
        {
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

        private MctResult<TdesResultWithIvs> DoTdesMctWithIvs(IMonteCarloTester<Common.Symmetric.MCTResult<AlgoArrayResponseWithIvs>, AlgoArrayResponseWithIvs> cipher, TdesParameters param)
        {
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
