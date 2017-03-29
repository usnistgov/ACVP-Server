using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DRBG.Enums;

namespace NIST.CVP.Generation.DRBG
{
    public class DrbgParameters
    {
        public DrbgMechanism Mechanism { get; set; }
        public DrbgMode Mode { get; set; }
        public bool DerFuncEnabled { get; set; }
        public bool PredResistanceEnabled { get; set; }
        public bool ReseedImplemented { get; set; }
        public int EntropyInputLen { get; set; }
        public int NonceLen { get; set; }
        public int PersoStringLen { get; set; }
        public int AdditionalInputLen { get; set; }
        public int ReturnedBitsLen { get; set; }
    }
}
