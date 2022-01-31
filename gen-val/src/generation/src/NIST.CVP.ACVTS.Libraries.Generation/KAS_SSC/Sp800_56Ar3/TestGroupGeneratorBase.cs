using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Ar3
{
    public abstract class TestGroupGeneratorBase<TTestGroup, TTestCase, TDomainParameters, TKeyPair> : ITestGroupGeneratorAsync<Parameters, TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
        where TDomainParameters : IDsaDomainParameters
        where TKeyPair : IDsaKeyPair
    {
        private static readonly string[] TestTypes =
        {
            "AFT",
            "VAL"
        };

        private int _registeredSchemeCount;

        public async Task<List<TTestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var schemes = parameters.Scheme.GetRegisteredSchemes().ToList();
            _registeredSchemeCount = schemes.Count();

            List<TTestGroup> groups = new List<TTestGroup>();

            foreach (var scheme in schemes)
            {
                var roleQueue = new ShuffleQueue<KeyAgreementRole>(scheme.KasRole.ToList());
                var testTypeQueue = new ShuffleQueue<string>(TestTypes.ToList());
                var dpGenerationQueue = GetDpGenerationQueue(parameters.DomainParameterGenerationMethods);

                var maxCount = new[]
                {
                    roleQueue.OriginalListCount,
                    testTypeQueue.OriginalListCount,
                    dpGenerationQueue.OriginalListCount.CeilingDivide(_registeredSchemeCount)
                }.Max();

                for (var i = 0; i < maxCount; i++)
                {
                    var role = roleQueue.Pop();
                    var testType = testTypeQueue.Pop();
                    var dpGeneration = dpGenerationQueue.Pop();

                    groups.Add(new TTestGroup()
                    {
                        IsSample = parameters.IsSample,
                        Scheme = scheme.Scheme,
                        KasAlgorithm = scheme.UnderlyingAlgorithm,
                        KasMode = KasMode.NoKdfNoKc,
                        KasRole = role,
                        TestType = testType,
                        DomainParameterGenerationMode = dpGeneration,
                        HashFunctionZ = parameters.HashFunctionZ
                    });
                }
            }

            await GenerateDomainParametersAsync(groups);
            await GenerateKeysPerDomainParametersAsync(groups);
            return groups;
        }

        protected abstract ShuffleQueue<KasDpGeneration> GetDpGenerationQueue(KasDpGeneration[] dpGeneration);
        protected abstract Task GenerateDomainParametersAsync(List<TTestGroup> testGroups);
        protected abstract Task GenerateKeysPerDomainParametersAsync(List<TTestGroup> groupList);
    }
}
