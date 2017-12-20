using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.IKEv1.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.IKEv1
{
    public interface IIkeV1Factory
    {
        IIkeV1 GetIkeV1Instance(AuthenticationMethods authMethods, HashFunction hash);
    }
}
