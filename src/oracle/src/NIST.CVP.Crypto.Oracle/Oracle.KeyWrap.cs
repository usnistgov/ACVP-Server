using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public AesResult GetAesKeyWrapCase() => throw new NotImplementedException();
        public AesResult GetAesKeyWrapWithPaddingCase() => throw new NotImplementedException();
        public TdesResult GetTdesKeyWrapCase() => throw new NotImplementedException();
    }
}
