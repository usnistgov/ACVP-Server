using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TPM;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TPM
{
    public class TpmFactory : ITpmFactory
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
