using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();

        foreach (var deterministicOption in parameters.Deterministic.Distinct())
        {
            foreach (var capability in parameters.Capabilities)
            {
                foreach (var signatureInterface in parameters.SignatureInterfaces.Distinct())
                {
                    foreach (var parameterSet in capability.ParameterSets.Distinct())
                    {
                        switch (signatureInterface)
                        {
                            case SignatureInterface.Internal:

                                foreach (var externalMu in parameters.ExternalMu)
                                {
                                    var testGroup = new TestGroup
                                    {
                                        TestType = "AFT",
                                        ParameterSet = parameterSet,
                                        Deterministic = deterministicOption,
                                        SignatureInterface = signatureInterface,
                                        ExternalMu = externalMu,
                                        MessageLength = capability.MessageLength
                                    };

                                    testGroups.Add(testGroup);
                                }

                                break;

                            case SignatureInterface.External:

                                foreach (var preHash in parameters.PreHash)
                                {
                                    var testGroup = new TestGroup
                                    {
                                        TestType = "AFT",
                                        ParameterSet = parameterSet,
                                        Deterministic = deterministicOption,
                                        SignatureInterface = signatureInterface,
                                        PreHash = preHash,
                                        HashFunctions = capability.HashAlgs,
                                        MessageLength = capability.MessageLength,
                                        ContextLength = capability.ContextLength
                                    };

                                    testGroups.Add(testGroup);
                                }

                                break;


                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        return Task.FromResult(testGroups);
    }
}
