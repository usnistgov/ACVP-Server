﻿using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigGen
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };

        public Capability[] Capabilities { get; set; }
        public bool ComponentTest { get; set; }
    }

    public class Capability
    {
        public string[] Curve { get; set; }
        public string[] HashAlg { get; set; }
    }
}