using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public Capability[] Capabilities { get; set; }
        public bool Pure { get; set; } = true;
        public bool PreHash { get; set; } = false;
    }

    public class Capability
    {
        public string[] Curve { get; set; }
    }
}
