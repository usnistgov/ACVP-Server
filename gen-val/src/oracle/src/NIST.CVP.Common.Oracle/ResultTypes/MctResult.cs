using System.Collections.Generic;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class MctResult<T> : IResult
    {
        public List<T> Results { get; set; } = new List<T>();
        public T Seed { get; set; }
    }
}
