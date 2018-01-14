using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.MAC;

namespace NIST.CVP.Crypto.Common.KDF
{
    public interface IKdfFactory
    {
        IKdf GetKdfInstance(KdfModes kdfMode, MacModes macMode, CounterLocations counterLocation, int counterLength = 0);

        IMac GetMacInstance(MacModes mode);
    }
}
