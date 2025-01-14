using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;

public class Parameters : IParameters
{
    public int VectorSetId { get; set; }
    public string Algorithm { get; set; }
    public string Mode { get; set; }
    public string Revision { get; set; }
    public bool IsSample { get; set; }
    public string[] Conformances { get; set; }
    
    public SlhdsaParameterSet[] ParameterSets { get; set; }
}
