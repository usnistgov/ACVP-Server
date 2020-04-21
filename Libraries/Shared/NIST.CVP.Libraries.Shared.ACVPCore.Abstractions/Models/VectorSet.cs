using System.Collections.Generic;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
    public class VectorSet : VectorSetLite
    {
        public List<VectorSetJsonFileTypes> JsonFilesAvailable { get; set; } = new List<VectorSetJsonFileTypes>();
        public VectorSetResetOption ResetOption { get; set; }
    }
}