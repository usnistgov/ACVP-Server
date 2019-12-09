using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class CounterParameters<T>
    {
        public T Parameters { get; set; }
        public bool Overflow { get; set; }
        public bool Incremental { get; set; }
    }
}
