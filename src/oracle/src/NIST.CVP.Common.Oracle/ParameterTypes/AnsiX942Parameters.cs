using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class AnsiX942Parameters
    {
        public HashFunction HashAlg { get; set; }
        public int ZzLen { get; set; }
        public int OtherIntoLen { get; set; }
        public int KeyLen { get; set; }
        public AnsiX942Types KdfMode { get; set; }
    }
}
