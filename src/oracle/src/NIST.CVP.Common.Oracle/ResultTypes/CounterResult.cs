using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class CounterResult : AesResult, ICryptoResult
    {
        public List<BitString> IVs { get; set; }
    }
}
