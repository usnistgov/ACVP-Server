using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.Crypto.TPM
{
    public class TpmFactory
    {
        private readonly IHmacFactory _hmacFactory;

        public TpmFactory(IHmacFactory hmacFactory)
        {
            _hmacFactory = hmacFactory;
        }

        public ITpm GetTpm()
        {
            // Only allows HMAC-SHA1
            var sha1Hmac = _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

            return new Tpm(sha1Hmac);
        }
    }
}
