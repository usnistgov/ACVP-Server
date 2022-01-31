using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.IKEv1
{
    public class IkeV1Factory : IIkeV1Factory
    {
        private readonly IHmacFactory _hmacFactory;
        private readonly IShaFactory _shaFactory;

        public IkeV1Factory(IHmacFactory hmacFactory, IShaFactory shaFactory)
        {
            _hmacFactory = hmacFactory;
            _shaFactory = shaFactory;
        }

        public IIkeV1 GetIkeV1Instance(AuthenticationMethods authMethods, HashFunction hash)
        {
            var sha = _shaFactory.GetShaInstance(hash);
            var hmac = _hmacFactory.GetHmacInstance(hash);

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
