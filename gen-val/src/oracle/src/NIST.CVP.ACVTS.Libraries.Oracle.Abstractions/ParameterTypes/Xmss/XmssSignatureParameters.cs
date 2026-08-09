using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;

public class XmssSignatureParameters
{
    public XmssMode XmssMode { get; set; }
    public IXmssKeyPair XmssKeyPair { get; set; }
    public int MessageLength { get; set; }
    public int Idx { get; set; }
    public XmssSignatureDisposition Disposition { get; set; }
}
