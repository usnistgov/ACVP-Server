using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF
{
    public interface IKdfFactory
    {
        IKdf GetKdfInstance(KdfModes kdfMode, MacModes macMode, CounterLocations counterLocation, int counterLength = 0);

        IMac GetMacInstance(MacModes mode);
    }
}
