using System.Collections.Generic;
using NIST.CVP.Algorithms.External;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject
    {
        public IEnumerable<AlgorithmBase> AlgorithmList { get; set; }
    }
}