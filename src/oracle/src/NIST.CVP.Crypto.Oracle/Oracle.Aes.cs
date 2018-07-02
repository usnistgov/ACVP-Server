using System;
using System.Linq;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private AesResult DoSimpleAes(IModeBlockCipher<SymmetricCipherResult> cipher, AesParameters param)
        {
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

        private MctResult<AesResult> DoSimpleAesMct(IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> cipher, AesParameters param)
        {
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

        public AesResult GetAesCbcCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new CbcBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesEcbCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new EcbBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesOfbCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new OfbBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesXtsCase(AesParameters param)
        {
            // Check Pool first
            var cipher = new XtsBlockCipher(new AesEngine());
            return DoSimpleAes(cipher, param);
        }

        public AesResult GetAesCfbCase(AesParameters param) => throw new NotImplementedException();
        public AesResult GetAesCtrCase(AesParameters param) => throw new NotImplementedException();

        public MctResult<AesResult> GetAesCbcMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesCbc(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesEcbMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesEcb(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesOfbMctCase(AesParameters param)
        {
            var cipher = new MonteCarloAesOfb(new BlockCipherEngineFactory(), new ModeBlockCipherFactory(), new AesMonteCarloKeyMaker());
            return DoSimpleAesMct(cipher, param);
        }

        public MctResult<AesResult> GetAesCfbMctCase(AesParameters param) => throw new NotImplementedException();
    }
}
