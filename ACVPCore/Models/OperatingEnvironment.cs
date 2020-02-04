using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class OperatingEnvironment
    {
        public long ID;
        public string Name { get; set; }
        public List<DependencyLite> Dependencies { get; set; } = new List<DependencyLite>();
    }
}
