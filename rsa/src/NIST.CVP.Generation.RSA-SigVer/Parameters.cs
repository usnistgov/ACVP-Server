using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public string[] SigGenModes { get; set; }
        public int[] Moduli { get; set; }
        public CapabilityObject[] Capabilities { get; set; }
        public bool CRT_Form { get; set; } = false;
    }

    public class CapabilityObject
    {
        public string HashAlg { get; set; }
        public int SaltLen { get; set; } = 0;
    }
}
