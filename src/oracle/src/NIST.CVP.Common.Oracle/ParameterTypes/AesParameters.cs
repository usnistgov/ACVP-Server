using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AesParameters
    {
        public int KeyLength { get; set; }
        public int DataLength { get; set; }
        public string Direction { get; set; }
    }
}
