using System;
using NIST.CVP.Crypto.Common.KAS.KDF;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfFactory : IKdfFactory
    {
        private readonly IKdfVisitor _visitor;

        public KdfFactory(IKdfVisitor visitor)
        {
            _visitor = visitor;
        }
        
        public IKdf GetKdf()
        {
            return new Kdf(_visitor);
        }
    }
}