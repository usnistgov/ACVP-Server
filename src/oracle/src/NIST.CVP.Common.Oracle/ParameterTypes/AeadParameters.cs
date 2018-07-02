using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AeadParameters
    {
        public int KeyLength { get; set; }
        public int IvLength { get; set; }
        public int DataLength { get; set; }
        public int AadLength { get; set; }
        public int TagLength { get; set; }
    }
}
