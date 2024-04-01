using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.KeyGen;

public class Parameters : IParameters
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; }
    public string Mode { get; set; }
    public string Revision { get; set; }
    public bool IsSample { get; set; }
    public string[] Conformances { get; set; }
    
    public KyberParameterSet[] ParameterSets { get; set; }
}
