using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.SHA2;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private readonly SHA _sha = new SHA(new SHAFactory());
        private SHA_MCT _shaMct;

        public HashResult GetShaCase(HashParameters param)
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

        public MctResult<HashResult> GetShaMctCase(HashParameters param)
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

        public HashResult GetSha3Case() => throw new NotImplementedException();
        public HashResult GetShakeCase() => throw new NotImplementedException();

        public MctResult<HashResult> GetSha3MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetShakeMctCase() => throw new NotImplementedException();

        public HashResult GetShakeVotCase() => throw new NotImplementedException();
    }
}
