using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class AnsiX942KdfResult
    {
        public BitString DerivedKey { get; set; }
        public BitString OtherInfo { get; set; }
        public BitString Zz { get; set; }
    }
}
