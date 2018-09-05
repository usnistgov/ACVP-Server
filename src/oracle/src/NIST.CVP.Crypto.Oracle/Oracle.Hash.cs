using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.TupleHash;
using System;
using NIST.CVP.Math;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Cshake;
using NIST.CVP.Orleans.Grains.Interfaces.Hash;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly SHA _sha = new SHA(new SHAFactory());
        private SHA_MCT _shaMct;
        private readonly SHA3.SHA3 _sha3 = new SHA3.SHA3(new SHA3Factory());
        private SHA3_MCT _sha3Mct;
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
            var grain = _clusterClient.GetGrain<IOracleObserverCShakeCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<CShakeResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<CShakeResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverParallelHashCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<ParallelHashResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<ParallelHashResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
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
            var grain = _clusterClient.GetGrain<IOracleObserverCShakeMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<CShakeResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<CShakeResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverParallelHashMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<ParallelHashResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<ParallelHashResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param)
        {
            return await _taskFactory.StartNew(() => GetTupleHashMctCase(param));
        }
    }
}
