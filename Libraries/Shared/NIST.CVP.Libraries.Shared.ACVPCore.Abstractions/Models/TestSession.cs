using System;
using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class TestSession : TestSessionLite
    {
        public DateTime? PassedOn { get; set; }
        public bool Publishable { get; set; }
        public bool Published { get; set; }
        public bool IsSample { get; set; }
        public long? UserID { get; set; }       //Nullable because old test sessions don't have it
        public string UserName { get; set; }
        public List<VectorSet> VectorSets { get; set; } = new List<VectorSet>();
    }
}