using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Common.Oracle
{
    public interface IFountain<T>
    {
        void FillPool(Pool<T> pool);
    }
}
