using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Lms;

public class LmsSignatureParameters
{
    public LmsMode LmsMode { get; set; }
    public LmOtsMode LmOtsMode { get; set; }
    public ILmsKeyPair LmsKeyPair { get; set; }
    public int MessageLength { get; set; }
    public LmsSignatureDisposition Disposition { get; set; }
}
