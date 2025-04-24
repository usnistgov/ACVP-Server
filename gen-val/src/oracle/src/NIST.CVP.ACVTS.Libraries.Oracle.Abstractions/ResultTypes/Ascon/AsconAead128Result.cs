using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;

public class AsconAead128Result
{
    public BitString Ciphertext { get; set; }
    public BitString Tag { get; set; }
    public BitString Key { get; set; }
    public BitString AD { get; set; }
    public BitString Nonce { get; set; }
    public BitString Plaintext { get; set; }
    public BitString SecondKey { get; set; } = null;
}
