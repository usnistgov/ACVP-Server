using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.Common.KDF.Components.TPM
{
    public interface ITpmFactory
    {
        ITpm GetTpm();
    }
}
