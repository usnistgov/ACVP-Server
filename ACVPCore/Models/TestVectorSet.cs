using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class TestVectorSet : TestVectorSetLite
    {
        public List<VectorSetJsonFileTypes> JsonFilesAvailable { get; set; } = new List<VectorSetJsonFileTypes>();
    }
}