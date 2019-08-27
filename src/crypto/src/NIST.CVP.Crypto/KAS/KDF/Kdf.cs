using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class Kdf : IKdf
    {
        private readonly IKdfVisitor _visitor;

        public Kdf(IKdfVisitor visitor)
        {
            _visitor = visitor;
        }
        
        public KasKdf KdfType { get; private set; }
        public KdfResult DeriveKey(IKdfParameter param)
        {
            if (param == null)
            {
                throw new ArgumentNullException($"{nameof(param)} for DeriveKey cannot be null.");
            }

            KdfType = param.KdfType;
            
            return param.AcceptKdf(_visitor);
        }
    }
}