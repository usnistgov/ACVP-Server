using System.Collections;
using System.Collections.Generic;

namespace ACVPCore.Models
{
    public class WrappedEnumerable<T>
    {
        public IEnumerable<T> Data { get; }

        public WrappedEnumerable(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}