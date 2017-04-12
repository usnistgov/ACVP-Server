using NIST.CVP.Crypto.DRBG.Enums;

namespace NIST.CVP.Crypto.DRBG
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
