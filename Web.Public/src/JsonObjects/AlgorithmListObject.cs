using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.External;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject
    {
        public IEnumerable<AlgorithmBase> AlgorithmList { get; set; }
    }
}