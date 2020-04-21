using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class OperatingEnvironment
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public List<DependencyLite> Dependencies { get; set; } = new List<DependencyLite>();
    }
}
