﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var cipher in parameters.Cipher)
            {
                var modeAsEnum = EnumHelpers.GetEnumFromEnumDescription<Cipher>(cipher);
                foreach (var hash in parameters.HashAlg)
                {
                    list.Add(new TestGroup
                    {
                        Cipher = modeAsEnum,
                        HashAlg = ShaAttributes.GetHashFunctionFromName(hash)
                    });
                }
            }

            return Task.FromResult(list);
        }
    }
}
