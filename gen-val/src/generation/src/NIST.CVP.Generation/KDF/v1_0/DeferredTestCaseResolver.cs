﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF.v1_0
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolverAsync<TestGroup, TestCase, KdfResult>
    {
        private readonly IOracle _oracle;

        public DeferredTestCaseResolver(IOracle oracle)
        {
            _oracle = oracle;
        }

        public async Task<KdfResult> CompleteDeferredCryptoAsync(TestGroup group, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new KdfParameters
            {
                KeyOutLength = group.KeyOutLength,
                Mode = group.KdfMode,
                MacMode = group.MacMode,
                CounterLocation = group.CounterLocation,
                CounterLength = group.CounterLength
            };

            var fullParam = new KdfResult
            {
                Iv = serverTestCase.IV,
                KeyIn = serverTestCase.KeyIn,
                FixedData = iutTestCase.FixedData,
                BreakLocation = iutTestCase.BreakLocation
            };

            return await _oracle.CompleteDeferredKdfCaseAsync(param, fullParam);
        }
    }
}