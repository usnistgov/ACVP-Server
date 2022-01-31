using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class MctResult<T> : IResult
    {
        public List<T> Results { get; set; } = new List<T>();
        public T Seed { get; set; }
    }
}
