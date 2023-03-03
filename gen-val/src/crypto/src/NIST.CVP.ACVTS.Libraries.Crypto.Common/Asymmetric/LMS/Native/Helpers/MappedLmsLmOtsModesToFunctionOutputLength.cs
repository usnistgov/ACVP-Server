using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers
{
    /// <summary>
    /// The <see cref="ModeValues"/> and output length of a LMS/LM-OTS parameter.
    /// </summary>
    /// <param name="Mode">The output function (SHA2 or SHAKE)</param>
    /// <param name="OutputLength">The output length (24 or 32 bytes)</param>
    /// <param name="LmsModes">The applicable <see cref="LmsModes"/> that were registered for this hash/output length</param>
    /// <param name="LmOtsModes">The applicable <see cref="LmOtsModes"/> that were registered for this hash/output length</param>
    public record MappedLmsLmOtsModesToFunctionOutputLength(ModeValues Mode, int OutputLength, List<LmsMode> LmsModes, List<LmOtsMode> LmOtsModes);
}
