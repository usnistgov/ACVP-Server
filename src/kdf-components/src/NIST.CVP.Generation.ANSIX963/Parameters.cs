﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ANSIX963
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public MathDomain SharedInfoLength { get; set; }
        public int[] FieldSize { get; set; }
        public MathDomain KeyDataLength { get; set; }
        public string[] HashAlg { get; set; }
    }
}