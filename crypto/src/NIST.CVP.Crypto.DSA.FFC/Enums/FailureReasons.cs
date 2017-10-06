using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.Enums
{
    public enum PQFailureReasons
    {
        None,
        ModifyP,
        ModifyQ,
        ModifySeed
    }

    public enum GFailureReasons
    {
        None,
        ModifyG
    }

    public enum SigFailureReasons
    {
        None,
        ModifyS,
        ModifyMessage,
        ModifyR,
        ModifyKey
    }
}
