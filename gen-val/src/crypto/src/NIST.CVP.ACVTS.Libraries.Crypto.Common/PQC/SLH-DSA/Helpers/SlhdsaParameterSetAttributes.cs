using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Helpers;

public record SlhdsaParameterSetAttributes(SlhdsaParameterSet SlhdsaParameterSet, int N, int H, int D, int HPrime, int A, int K, int LGw, int W, int Len, int Len1, int Len2, int M, SecurityLevel SecurityLevel, int PKBytes, int SigBytes, ModeValues ShaMode)
{
    /// <summary>
    /// The parameter set attributes being described. These are the "SLH-DSA parameter sets" described in Section 10,
    /// Table 1 of FIPS 205. 
    /// </summary>
    
    public SlhdsaParameterSet SlhdsaParameterSet { get; } = SlhdsaParameterSet;
    
    public int N { get; } = N;
    public int H { get; } = H; 
    public int D { get; } = D;
    // The height of a Merkle tree. See Section 6.
    public int HPrime { get; } = HPrime;
    public int A { get; } = A;
    public int K { get; } = K;
    public int LGw { get; } = LGw;
    // The w defined in equation (5.1) of the FIPS. w isn't really part of a parameter set definition; rather, it is a
    // derived value. But it's convenient for it to live here. 
    public int W { get; } = W;
    // len from section 5, equation (5.4) of the FIPS
    public int Len { get; } = Len;
    // len1 from section 5, equation (5.2) of the FIPS
    public int Len1 { get; } = Len1;
    // len2 from section 5, equation (5.3) of the FIPS
    public int Len2 { get; } = Len2;
    public int M { get; } = M;
    // otherwise referred to as "Security Category"
    public SecurityLevel SecurityLevel { get; } = SecurityLevel;
    public int PKBytes { get; } = PKBytes;
    public int SigBytes { get; } = SigBytes;
    public ModeValues ShaMode { get; } = ShaMode;
    
}
