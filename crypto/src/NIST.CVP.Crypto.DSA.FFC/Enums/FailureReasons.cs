using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.Enums
{
    public enum PQFailureReasons
    {
        None1,
        None2,
        ModifyP,
        ModifyQ,
        ModifySeed,
    }

    public enum GFailureReasons
    {
        None1,
        None2,
        ModifyG1,
        ModifyG2,
        ModifyG3
    }
}
