using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public HashResult GetSha1Case() => throw new NotImplementedException();
        public HashResult GetSha2Case() => throw new NotImplementedException();
        public HashResult GetSha3Case() => throw new NotImplementedException();
        public HashResult GetShakeCase() => throw new NotImplementedException();

        public MctResult<HashResult> GetSha1MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetSha2MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetSha3MctCase() => throw new NotImplementedException();
        public MctResult<HashResult> GetShakeMctCase() => throw new NotImplementedException();

        public HashResult GetShakeVotCase() => throw new NotImplementedException();
    }
}
