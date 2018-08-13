using System.Collections.Generic;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class MctResult<T>
    {
        public List<T> Results { get; set; } = new List<T>();
    }
}
