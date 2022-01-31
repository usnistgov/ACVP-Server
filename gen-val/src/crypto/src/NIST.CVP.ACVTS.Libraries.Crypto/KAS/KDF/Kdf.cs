using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
{
    public class Kdf : IKdf
    {
        private readonly IKdfVisitor _visitor;

        public Kdf(IKdfVisitor visitor)
        {
            _visitor = visitor;
        }

        public Kda KdfType { get; private set; }
        public KdfResult DeriveKey(IKdfParameter param, BitString fixedInfo)
        {
            if (param == null)
            {
                throw new ArgumentNullException($"{nameof(param)} for DeriveKey cannot be null.");
            }

            KdfType = param.KdfType;

            return param.AcceptKdf(_visitor, fixedInfo);
        }
    }
}
