using System.Collections.Generic;

namespace NIST.CVP.Enumerables
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