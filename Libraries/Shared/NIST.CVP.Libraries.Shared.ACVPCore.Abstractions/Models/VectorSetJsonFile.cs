using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class VectorSetJsonFile
    {
        public VectorSetJsonFileTypes Type { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
