using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF
{
    public class KdfMutliExpansion : IKdfMultiExpansion
    {
        private readonly IKdfMultiExpansionVisitor _visitor;

        public KdfMutliExpansion(IKdfMultiExpansionVisitor visitor)
        {
            _visitor = visitor;
        }

        public Kda KdfType { get; private set; }
        public KdfMultiExpansionResult DeriveKey(IKdfMultiExpansionParameter param)
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
