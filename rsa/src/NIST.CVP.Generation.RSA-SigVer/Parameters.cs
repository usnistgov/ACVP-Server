using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class Parameters : IParameters
    {
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public bool IsSample { get; set; }

        public AlgSpecs[] Capabilities { get; set; }
        public string PubExpMode { get; set; }
        public string FixedPubExpValue { get; set; } = "";
        public string KeyFormat { get; set; }
    }

    public class AlgSpecs
    {
        public string SigType { get; set; }
        public CapSigType[] ModuloCapabilities { get; set; }
    }

    public class CapSigType
    {
        public int Modulo { get; set; }
        public HashPair[] HashPairs { get; set; }
    }

    public class HashPair
    {
        public string HashAlg { get; set; }
        public int SaltLen { get; set; } = 0;
    }
}
