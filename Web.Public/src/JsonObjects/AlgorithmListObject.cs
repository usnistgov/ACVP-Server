using System.Collections.Generic;
using ACVPCore.Algorithms.External;

namespace Web.Public.JsonObjects
{
    public class AlgorithmListObject : IJsonObject
    {
        public string AcvVersion { get; set; }
        public IEnumerable<AlgorithmBase> AlgorithmList { get; set; }
    }
}