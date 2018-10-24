using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.TupleHash;
using NIST.CVP.Math;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private HashResult GetShaCase(ShaParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            var result = new SHA(new SHAFactory()).HashMessage(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }
         
            return new HashResult
            {
                Message = message,
                Digest = result.Digest
            };
        }

        private MctResult<HashResult> GetShaMctCase(ShaParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var rand = new Random800_90();
            var shaMct = new SHA_MCT(new SHA(new SHAFactory()));

            var message = rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = shaMct.MCTHash(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResult>
            {
                Seed = new HashResult {Message = message},
                Results = result.Response.ConvertAll(element =>
                    new HashResult {Message = element.Message, Digest = element.Digest})
            };
        }

        private HashResult GetSha3Case(Sha3Parameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            var result = new SHA3.SHA3(new SHA3Factory()).HashMessage(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new HashResult
            {
                Message = message,
                Digest = result.Digest
            };
        }

        private MctResult<HashResult> GetSha3MctCase(Sha3Parameters param)
        {
            var poolBoy = new PoolBoy<MctResult<HashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.SHA3_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = new SHA3_MCT(new SHA3.SHA3()).MCTHash(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResult>
            {
                Seed = new HashResult { Message = message },
                Results = result.Response.ConvertAll(element =>
                    new HashResult { Message = element.Message, Digest = element.Digest })
            };
        }

        private MctResult<HashResult> GetShakeMctCase(ShakeParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = new SHAKE_MCT(new SHA3.SHA3()).MCTHash(param.HashFunction, message, param.OutputLengths);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResult>
            {
                Seed = new HashResult { Message = message },
                Results = result.Response.ConvertAll(element =>
                    new HashResult { Message = element.Message, Digest = element.Digest })
            };
        }

        private CShakeResult GetCShakeCase(CShakeParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            var cshake = new CSHAKE.CSHAKE(new CSHAKEFactory());
            if (param.HexCustomization)
            {
                customizationHex = rand.GetRandomBitString(param.CustomizationLength);
                result = cshake.HashMessage(param.HashFunction, message, customizationHex, param.FunctionName);
            }
            else
            {
                customization = rand.GetRandomString(param.CustomizationLength);
                result = cshake.HashMessage(param.HashFunction, message, customization, param.FunctionName);
            }

            if (!result.Success)
            {
                throw new Exception();
            }

            return new CShakeResult
            {
                Message = message,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex,
                FunctionName = param.FunctionName
            };
        }

        private MctResult<CShakeResult> GetCShakeMctCase(CShakeParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<CShakeResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.CSHAKE_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = new CSHAKE_MCT(new CSHAKE.CSHAKE())
                .MCTHash(param.HashFunction, message, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<CShakeResult>
            {
                Seed = new CShakeResult { Message = message },
                Results = result.Response.ConvertAll(element =>
                    new CShakeResult { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private ParallelHashResult GetParallelHashCase(ParallelHashParameters param)
        {
            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            var parallelHash = new ParallelHash.ParallelHash();
            if (param.HexCustomization)
            {
                customizationHex = rand.GetRandomBitString(param.CustomizationLength);
                result = parallelHash.HashMessage(param.HashFunction, message, param.BlockSize, customizationHex);
            }
            else
            {
                customization = rand.GetRandomString(param.CustomizationLength);
                result = parallelHash.HashMessage(param.HashFunction, message, param.BlockSize, customization);
            }

            if (!result.Success)
            {
                throw new Exception();
            }

            return new ParallelHashResult
            {
                Message = message,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex,
                BlockSize = param.BlockSize
            };
        }

        private MctResult<ParallelHashResult> GetParallelHashMctCase(ParallelHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<ParallelHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.PARALLEL_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var rand = new Random800_90();
            var message = rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = new ParallelHash_MCT(new ParallelHash.ParallelHash())
                .MCTHash(param.HashFunction, message, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<ParallelHashResult>
            {
                Seed = new ParallelHashResult { Message = message },
                Results = result.Response.ConvertAll(element =>
                    new ParallelHashResult { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private TupleHashResult GetTupleHashCase(TupleHashParameters param)
        {
            var rand = new Random800_90();
            var tuple = new List<BitString>();

            if (param.SemiEmptyCase)
            {
                for (int i = 0; i < param.TupleSize; i++)
                {
                    if (rand.GetRandomInt(0, 2) == 1)  // either 1 or 0
                    {
                        tuple.Add(rand.GetRandomBitString(GetRandomValidLength(param.BitOrientedInput)));
                    }
                    else
                    {
                        tuple.Add(new BitString(""));
                    }
                }
            }
            else if (param.LongRandomCase)
            {
                for (int i = 0; i < param.TupleSize; i++)
                {
                    tuple.Add(rand.GetRandomBitString(GetRandomValidLength(param.BitOrientedInput)));
                }
            }
            else
            {
                for (int i = 0; i < param.TupleSize; i++)
                {
                    tuple.Add(rand.GetRandomBitString(param.MessageLength));
                }
            }
            
            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            var tupleHash = new TupleHash.TupleHash();
            if (param.HexCustomization)
            {
                customizationHex = rand.GetRandomBitString(param.CustomizationLength);
                result = tupleHash.HashMessage(param.HashFunction, tuple, customizationHex);
            }
            else
            {
                customization = rand.GetRandomString(param.CustomizationLength);
                result = tupleHash.HashMessage(param.HashFunction, tuple, customization);
            }

            if (!result.Success)
            {
                throw new Exception();
            }

            return new TupleHashResult
            {
                Tuple = tuple,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex
            };
        }

        private MctResult<TupleHashResult> GetTupleHashMctCase(TupleHashParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TupleHashResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TUPLE_HASH_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }
            
            var rand = new Random800_90();
            var tuple = new List<BitString>() { rand.GetRandomBitString(param.MessageLength) };

            // TODO isSample up in here?
            var result = new TupleHash_MCT(new TupleHash.TupleHash())
                .MCTHash(param.HashFunction, tuple, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<TupleHashResult>
            {
                Seed = new TupleHashResult { Tuple = tuple },
                Results = result.Response.ConvertAll(element =>
                    new TupleHashResult { Tuple = element.Tuple, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private int GetRandomValidLength(bool bitOriented)
        {
            var rand = new Random800_90();
            var length = rand.GetRandomInt(1, 513);
            if (!bitOriented)
            {
                while (length % 8 != 0)
                {
                    length++;
                }
            }
            return length;
        }

        public async Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            return await _taskFactory.StartNew(() => GetShaCase(param));
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            return await _taskFactory.StartNew(() => GetSha3Case(param));
        }

        public async Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param)
        {
            return await _taskFactory.StartNew(() => GetCShakeCase(param));
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            return await _taskFactory.StartNew(() => GetParallelHashCase(param));
        }

        public async Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param)
        {
            return await _taskFactory.StartNew(() => GetTupleHashCase(param));
        }

        public async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            return await _taskFactory.StartNew(() => GetShaMctCase(param));
        }

        public async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            return await _taskFactory.StartNew(() => GetSha3MctCase(param));
        }

        public async Task<MctResult<HashResult>> GetShakeMctCaseAsync(ShakeParameters param)
        {
            return await _taskFactory.StartNew(() => GetShakeMctCase(param));
        }

        public async Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param)
        {
            return await _taskFactory.StartNew(() => GetCShakeMctCase(param));
        }

        public async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            return await _taskFactory.StartNew(() => GetParallelHashMctCase(param));
        }

        public async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            return await _taskFactory.StartNew(() => GetTupleHashMctCase(param));
        }
    }
}
