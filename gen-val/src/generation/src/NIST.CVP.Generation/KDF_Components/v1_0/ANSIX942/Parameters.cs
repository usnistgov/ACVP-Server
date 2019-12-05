using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public string[] KdfType { get; set; }
        public MathDomain KeyLen { get; set; }
        public MathDomain OtherInfoLen { get; set; }
        public MathDomain ZzLen { get; set; }
        public string[] HashAlg { get; set; }
    }
}
