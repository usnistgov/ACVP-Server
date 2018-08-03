using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.TupleHash;
using System;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly SHA _sha = new SHA(new SHAFactory());
        private SHA_MCT _shaMct;
        private readonly SHA3.SHA3 _sha3 = new SHA3.SHA3(new SHA3Factory());
        private SHA3_MCT _sha3Mct;
        private readonly CSHAKE.CSHAKE _cSHAKE = new CSHAKE.CSHAKE(new CSHAKEFactory());
        private CSHAKE_MCT _cSHAKEMct;
        private readonly ParallelHash.ParallelHash _parallelHash = new ParallelHash.ParallelHash(new ParallelHashFactory());
        private ParallelHash_MCT _parallelHashMct;
        private readonly TupleHash.TupleHash _tupleHash = new TupleHash.TupleHash(new TupleHashFactory());
        private TupleHash_MCT _tupleHashMct;

        private HashResult GetShaCase(ShaParameters param)
        {
            var message = _rand.GetRandomBitString(param.MessageLength);

            var result = _sha.HashMessage(param.HashFunction, message);

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
            _shaMct = new SHA_MCT(_sha);

            var message = _rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = _shaMct.MCTHash(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new HashResult {Message = element.Message, Digest = element.Digest})
            };
        }

        private HashResult GetSha3Case(Sha3Parameters param)
        {
            var message = _rand.GetRandomBitString(param.MessageLength);

            var result = _sha3.HashMessage(param.HashFunction, message);

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
            _sha3Mct = new SHA3_MCT(_sha3);

            var message = _rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = _sha3Mct.MCTHash(param.HashFunction, message);

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new HashResult { Message = element.Message, Digest = element.Digest })
            };
        }

        private CShakeResult GetCShakeCase(CShakeParameters param)
        {
            var message = _rand.GetRandomBitString(param.MessageLength);

            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            if (param.HexCustomization)
            {
                customizationHex = _rand.GetRandomBitString(param.CustomizationLength);
                result = _cSHAKE.HashMessage(param.HashFunction, message, customizationHex, param.FunctionName);
            }
            else
            {
                customization = _rand.GetRandomString(param.CustomizationLength);
                result = _cSHAKE.HashMessage(param.HashFunction, message, customization, param.FunctionName);
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
            _cSHAKEMct = new CSHAKE_MCT(_cSHAKE);

            var message = _rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = _cSHAKEMct.MCTHash(param.HashFunction, message, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<CShakeResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new CShakeResult { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private ParallelHashResult GetParallelHashCase(ParallelHashParameters param)
        {
            var message = _rand.GetRandomBitString(param.MessageLength);

            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            if (param.HexCustomization)
            {
                customizationHex = _rand.GetRandomBitString(param.CustomizationLength);
                result = _parallelHash.HashMessage(param.HashFunction, message, param.BlockSize, customizationHex);
            }
            else
            {
                customization = _rand.GetRandomString(param.CustomizationLength);
                result = _parallelHash.HashMessage(param.HashFunction, message, param.BlockSize, customization);
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
            _parallelHashMct = new ParallelHash_MCT(_parallelHash);

            var message = _rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = _parallelHashMct.MCTHash(param.HashFunction, message, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<ParallelHashResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new ParallelHashResult { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private TupleHashResult GetTupleHashCase(TupleHashParameters param)
        {
            var tuple = new List<BitString>();

            if (param.SemiEmptyCase)
            {
                for (int i = 0; i < param.TupleSize; i++)
                {
                    if (_rand.GetRandomInt(0, 2) == 1)  // either 1 or 0
                    {
                        tuple.Add(_rand.GetRandomBitString(GetRandomValidLength(param.BitOrientedInput)));
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
                    tuple.Add(_rand.GetRandomBitString(GetRandomValidLength(param.BitOrientedInput)));
                }
            }
            else
            {
                for (int i = 0; i < param.TupleSize; i++)
                {
                    tuple.Add(_rand.GetRandomBitString(param.MessageLength));
                }
            }
            
            Common.Hash.HashResult result;
            BitString customizationHex = null;
            string customization = "";
            if (param.HexCustomization)
            {
                customizationHex = _rand.GetRandomBitString(param.CustomizationLength);
                result = _tupleHash.HashMessage(param.HashFunction, tuple, customizationHex);
            }
            else
            {
                customization = _rand.GetRandomString(param.CustomizationLength);
                result = _tupleHash.HashMessage(param.HashFunction, tuple, customization);
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
            _tupleHashMct = new TupleHash_MCT(_tupleHash);

            var tuple = new List<BitString>() { _rand.GetRandomBitString(param.MessageLength) };

            // TODO isSample up in here?
            var result = _tupleHashMct.MCTHash(param.HashFunction, tuple, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<TupleHashResult>
            {
                Results = result.Response.ConvertAll(element =>
                    new TupleHashResult { Tuple = element.Tuple, Digest = element.Digest, Customization = element.Customization })
            };
        }

        private int GetRandomValidLength(bool bitOriented)
        {
            var length = _rand.GetRandomInt(1, 513);
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
