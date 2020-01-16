using System;
using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class TestSession : TestSessionLite
    {
        public DateTime? PassedOn { get; set; }
        public bool Publishable { get; set; }
        public bool Published { get; set; }
        public bool IsSample { get; set; }
        public List<TestVectorSetLite> VectorSets { get; set; } = new List<TestVectorSetLite>();
    }
}