using System.Collections.Generic;

namespace PoolBitStringConverter.Models
{
    internal class PoolValues
    {
        public bool HasAdditionalValues { get; set; }
        public List<PoolValue> Values { get; set; } = new List<PoolValue>();
    }
}