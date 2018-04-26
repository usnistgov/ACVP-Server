using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.IKEv1
{
    public class IkeV1Factory : IIkeV1Factory
    {
        public IIkeV1 GetIkeV1Instance(AuthenticationMethods authMethods, HashFunction hash)
        {
            var shaFactory = new ShaFactory();
            var sha = shaFactory.GetShaInstance(hash);
            var hmacFactory = new HmacFactory(shaFactory);
            var hmac = hmacFactory.GetHmacInstance(hash);

            switch (authMethods)
            {
                case AuthenticationMethods.Dsa:
                    return new DsaIkeV1(hmac);
                case AuthenticationMethods.Pke:
                    return new PkeIkeV1(hmac, sha);
                case AuthenticationMethods.Psk:
                    return new PskIkeV1(hmac);
                default:
                    throw new ArgumentException("No such authentication mode");
            }
        }
    }
}
