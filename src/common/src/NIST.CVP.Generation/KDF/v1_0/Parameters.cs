﻿using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF.v1_0
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
    }

    public class Capability
    {
        public string KdfMode { get; set; }
        public string[] MacMode{get; set; }
        public MathDomain SupportedLengths{ get; set; }
        public string[] FixedDataOrder { get; set; }
        public int[] CounterLength { get; set; } = {0};
        public bool SupportsEmptyIv { get; set; } = false;
    }
}