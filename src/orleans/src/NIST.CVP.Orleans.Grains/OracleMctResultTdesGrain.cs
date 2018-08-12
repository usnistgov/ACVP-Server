using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;

namespace NIST.CVP.Orleans.Grains
{
    
    public class OracleMctResultTdesGrain : 
        OracleGrainBase<MctResult<TdesResult>>, IOracleMctResultTdesGrain<MctResult<TdesResult>>
    {
        private readonly TdesMonteCarloFactory _tdesMctFactory = new TdesMonteCarloFactory(
            new BlockCipherEngineFactory(), new ModeBlockCipherFactory()
        );
        private readonly Random800_90 _rand = new Random800_90();

        private TdesParameters _param;

        public async Task<bool> BeginTdesMctCaseAsync(TdesParameters param)
        {
            _param = param;
            return await BeginGrainWorkAsync();
        }

        protected override Task DoWorkAsync()
        {
            var cipher = _tdesMctFactory.GetInstance(_param.Mode);
            var direction = BlockCipherDirections.Encrypt;
            if (_param.Direction.ToLower() == "decrypt")
            {
                direction = BlockCipherDirections.Decrypt;
            }

            var payload = _rand.GetRandomBitString(_param.DataLength);
            var key = TdesHelpers.GenerateTdesKey(_param.KeyingOption);
            var iv = _rand.GetRandomBitString(64);

            var blockCipherParams = new ModeBlockCipherParameters(direction, iv, key, payload);
            var result = cipher.ProcessMonteCarloTest(blockCipherParams);

            if (!result.Success)
            {
                // Log error somewhere
                throw new Exception();
            }

            Result = new MctResult<TdesResult>
            {
                Results = Array.ConvertAll(result.Response.ToArray(), element => new TdesResult
                {
                    Key = element.Keys,
                    Iv = element.IV,
                    PlainText = element.PlainText,
                    CipherText = element.CipherText
                }).ToList()
            };

            State = GrainState.CompletedWork;
            return Task.CompletedTask;
        }
    }
}