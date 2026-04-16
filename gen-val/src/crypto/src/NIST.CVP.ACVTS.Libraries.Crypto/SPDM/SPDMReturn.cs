using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SPDM;

public class SPDMReturn
{
    public BitString RequestDirectionHandshake { get; set; }
    public BitString ResponseDirectionHandshake { get; set; }
    public BitString RequestDirectionData { get; set; }
    public BitString ResponseDirectionData { get; set; }
    public BitString ExportMaster { get; set; }
}
