using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
    public class OperatingEnvironment
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public List<DependencyLite> Dependencies { get; set; } = new List<DependencyLite>();
    }
}
