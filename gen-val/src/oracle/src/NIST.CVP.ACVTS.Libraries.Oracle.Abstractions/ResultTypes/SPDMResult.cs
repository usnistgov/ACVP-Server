using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SPDM;
public class SPDMResult
{
    public BitString RequestDirectionHandshake { get; set; }
    public BitString ResponseDirectionHandshake { get; set; }
    public BitString RequestDirectionData { get; set; }
    public BitString ResponseDirectionData { get; set; }
    public BitString ExportMaster { get; set; }
    public BitString Key { get; set; }
    public BitString TH1 { get; set; } = null;
    public BitString TH2 { get; set; } = null;
}
