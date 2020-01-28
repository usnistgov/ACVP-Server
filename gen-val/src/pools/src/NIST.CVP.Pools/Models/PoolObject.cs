using System;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools.Models
{
    public class PoolObject<TResult>
        where TResult : IResult
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateLastUsed { get; set; }
        public long TimesUsed { get; set; }
        public TResult Value { get; set; }
    }
}
