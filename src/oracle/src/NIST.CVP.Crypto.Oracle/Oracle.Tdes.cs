using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public TdesResult GetTdesCbcCase() => throw new NotImplementedException();
        public TdesResult GetTdesCfbCase() => throw new NotImplementedException();
        public TdesResult GetTdesEcbCase() => throw new NotImplementedException();
        public TdesResult GetTdesOfbCase() => throw new NotImplementedException();

        public TdesResultWithIvs GetTdesCbcICase() => throw new NotImplementedException();
        public TdesResultWithIvs GetTdesOfbICase() => throw new NotImplementedException();

        public MctResult<TdesResult> GetTdesCbcMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesCfbMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesEcbMctCase() => throw new NotImplementedException();
        public MctResult<TdesResult> GetTdesOfbMctCase() => throw new NotImplementedException();

        public MctResult<TdesResultWithIvs> GetTdesCbcIMctCase() => throw new NotImplementedException();
        public MctResult<TdesResultWithIvs> GetTdesOfbIMctCase() => throw new NotImplementedException();
    }
}
