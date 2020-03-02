using System.Collections.Generic;
using ACVPCore.Algorithms.External;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject
    {
        public JwtObject Jwt { get; set; }
        public IEnumerable<AlgorithmBase> AlgorithmList { get; set; }
    }
}