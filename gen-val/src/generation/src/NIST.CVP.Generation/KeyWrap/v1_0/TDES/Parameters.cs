﻿using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KeyWrap.v1_0.TDES
{
    //TODO: This already exists in TDES_ECB generation. It would be good to use that as well as the AES one

    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; } = { };
        public int[] KeyLen { get; set; }
        public string[] Direction { get; set; }
        public string[] KwCipher { get; set; }
        public MathDomain PayloadLen { get; set; }
    }
}