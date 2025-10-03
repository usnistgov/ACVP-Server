using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;

public class AsconHashParameters
{
    public int MessageBitLength { get; set; }
    public int DigestBitLength { get; set; }
    public int CSBitLength { get; set; }
}
