using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;

public class AsconHashResult
{
    public BitString Digest { get; set; }
    public BitString Message { get; set; }
    public BitString CS { get; set; }
}
