using NIST.CVP.Crypto.KDF.Enums;
using NIST.CVP.Crypto.MAC;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.KDF
{
    public interface IKdfFactory
    {
        IKdf GetKdfInstance(KdfModes kdfMode, MacModes macMode, CounterLocations counterLocation, int counterLength = 0);

        IMac GetMacInstance(MacModes mode);
    }
}
