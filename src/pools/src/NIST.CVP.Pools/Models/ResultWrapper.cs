using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools.Models
{
    public class ResultWrapper<TResult>
        where TResult : IResult
    {
        public TResult Result { get; set; }
        public int TimesValueUsed { get; set; }
        public DateTime ValueCreated { get; set; }
        public DateTime? ValueUsed { get; set; }
    }
}
