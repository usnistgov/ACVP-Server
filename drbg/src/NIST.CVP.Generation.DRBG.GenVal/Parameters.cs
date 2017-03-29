using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG.GenVal
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        /// <summary>
        /// TODO should this be a string for testing multiple modes in the same vector set?
        /// </summary>
        public string Mode { get; set; }
        public bool DerFuncEnabled { get; set; }
        public bool PredResistanceEnabled { get; set; }
        public bool ReseedImplemented { get; set; }
        public int EntropyInputLen { get; set; }
        public int NonceLen { get; set; }
        public Range PersoStringLen { get; set; }
        public Range AdditionalInputLen { get; set; }
        public int ReturnedBitsLen { get; set; }
    }
}
