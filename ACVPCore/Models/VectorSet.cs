using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class VectorSet : VectorSetLite
    {
        public List<VectorSetJsonFileTypes> JsonFilesAvailable { get; set; } = new List<VectorSetJsonFileTypes>();
    }
}