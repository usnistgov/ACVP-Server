using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models
{
    public class VectorSet : VectorSetLite
    {
        public List<VectorSetJsonFile> JsonFilesAvailable { get; set; } = new List<VectorSetJsonFile>();
        public VectorSetResetOption ResetOption { get; set; }
        public long TestSessionID { get; set; }
    }
}