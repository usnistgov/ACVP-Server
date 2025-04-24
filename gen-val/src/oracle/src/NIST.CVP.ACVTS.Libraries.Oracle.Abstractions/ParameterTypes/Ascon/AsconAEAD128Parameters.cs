using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;

public class AsconAEAD128Parameters
{
    public int PayloadBitLength { get; set; }
    public int ADBitLength { get; set; }
    public int TruncationLength { get; set; }
    public bool NonceMasking { get; set; } 
}
