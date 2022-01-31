using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class AnsiX942KdfResult
    {
        public BitString DerivedKey { get; set; }
        public BitString OtherInfo { get; set; }
        public BitString Zz { get; set; }
        public BitString PartyUInfo { get; set; }
        public BitString PartyVInfo { get; set; }
        public BitString SuppPubInfo { get; set; }
        public BitString SuppPrivInfo { get; set; }
    }
}
