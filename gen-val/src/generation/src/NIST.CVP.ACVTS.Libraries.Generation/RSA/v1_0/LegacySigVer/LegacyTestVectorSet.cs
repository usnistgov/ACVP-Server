using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.LegacySigVer
{
    public class LegacyTestVectorSet : TestVectorSet
    {
        public override string Revision { get; set; } = "FIPS186-2";
    }
}
