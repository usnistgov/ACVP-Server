using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TPM
{
    public interface ITpmFactory
    {
        ITpm GetTpm();
    }
}
