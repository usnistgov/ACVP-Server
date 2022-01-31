using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class CounterParameters<T>
    {
        public T Parameters { get; set; }
        public bool Overflow { get; set; }
        public bool Incremental { get; set; }
    }
}
