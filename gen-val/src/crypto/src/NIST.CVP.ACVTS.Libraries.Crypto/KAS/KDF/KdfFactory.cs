using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
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
