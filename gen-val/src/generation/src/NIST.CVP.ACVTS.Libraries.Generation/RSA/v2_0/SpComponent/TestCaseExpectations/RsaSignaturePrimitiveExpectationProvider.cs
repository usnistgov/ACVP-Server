using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v2_0.SpComponent.TestCaseExpectations
{
    public class RsaSignaturePrimitiveExpectationProvider : TestCaseExpectationProviderBase<RsaSpDisposition>
    {
        public RsaSignaturePrimitiveExpectationProvider()
        {
            var expectationReasons = new List<RsaSpDisposition>
            {
                { RsaSpDisposition.MsgEqualN, 2 }, 
                { RsaSpDisposition.MsgGreaterNLessModulo, 2 }, 
                { RsaSpDisposition.None, 11 }
            };

            LoadExpectationReasons(expectationReasons);
        }
    }
}
