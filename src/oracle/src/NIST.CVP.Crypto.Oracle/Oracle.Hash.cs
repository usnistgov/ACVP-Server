using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Crypto.TupleHash;
using System;
using NIST.CVP.Math;
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

        public HashResult GetShaCase(ShaParameters param)
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

        public MctResult<HashResult> GetShaMctCase(ShaParameters param)
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

        public HashResult GetSha3Case(Sha3Parameters param)
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

        public MctResult<HashResult> GetSha3MctCase(Sha3Parameters param)
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

        public HashResultCSHAKE GetCShakeCase(CSHAKEParameters param)
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

            return new HashResultCSHAKE
            {
                Message = message,
                Digest = result.Digest,
                Customization = customization,
                CustomizationHex = customizationHex,
                FunctionName = param.FunctionName
            };
        }

        public MctResult<HashResultCSHAKE> GetCShakeMctCase(CSHAKEParameters param)
        {
            _cSHAKEMct = new CSHAKE_MCT(_cSHAKE);

            var message = _rand.GetRandomBitString(param.MessageLength);

            // TODO isSample up in here?
            var result = _cSHAKEMct.MCTHash(param.HashFunction, message, param.OutLens, true); // currently always a sample

            if (!result.Success)
            {
                throw new Exception();
            }

            return new MctResult<HashResultCSHAKE>
            {
                Results = result.Response.ConvertAll(element =>
                    new HashResultCSHAKE { Message = element.Message, Digest = element.Digest, Customization = element.Customization })
            };
        }

        public async Task<HashResult> GetShaCaseAsync(ShaParameters param)
        {
            return await _taskFactory.StartNew(() => GetShaCase(param));
        }

        public async Task<HashResult> GetSha3CaseAsync(Sha3Parameters param)
        {
            return await _taskFactory.StartNew(() => GetSha3Case(param));
        }

        public async Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param)
        {
            return await _taskFactory.StartNew(() => GetShaMctCase(param));
        }

        public async Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param)
        {
            return await _taskFactory.StartNew(() => GetSha3MctCase(param));
        }
    }
}
