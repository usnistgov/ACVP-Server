using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasSscAftDeferredResultIfc>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KasSscAftDeferredResultIfc> CompleteDeferredCryptoAsync(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            try
            {
                var result = await _oracle.CompleteDeferredKasIfcSscAftTestAsync(new KasSscAftDeferredParametersIfc()
                {
                    Modulo = serverTestGroup.Modulo,
                    Scheme = serverTestGroup.Scheme,
                    KasMode = serverTestGroup.KasMode,
                    IutKeyAgreementRole = serverTestGroup.KasRole,
                    HashFunctionZ = serverTestGroup.HashFunctionZ,

                    ServerKey = serverTestCase.ServerKey,
                    ServerC = serverTestCase.ServerC,
                    ServerZ = serverTestCase.ServerZ,

                    IutKey = serverTestCase.IutKey,
                    IutC = iutTestCase.IutC,
                    IutZ = iutTestCase.Z,

                });
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
