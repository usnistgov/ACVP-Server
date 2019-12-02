﻿using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class IkeV2KdfParameters
    {
        public int NInitLength { get; set; }
        public int NRespLength { get; set; }
        public int GirLength { get; set; }
        public HashFunction HashAlg { get; set; }
        public int DerivedKeyingMaterialLength { get; set; }
    }
}
