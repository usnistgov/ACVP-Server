using System.Collections.Generic;

namespace NIST.CVP.Libraries.Shared.Enumerables
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