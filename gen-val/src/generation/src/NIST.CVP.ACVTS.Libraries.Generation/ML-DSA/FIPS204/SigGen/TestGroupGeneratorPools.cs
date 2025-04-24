using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class TestGroupGeneratorPools() : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
{
    public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
    {
        var testGroups = new List<TestGroup>();
        
        // Skip if internal signature interface is not included
        if (!parameters.SignatureInterfaces.Contains(SignatureInterface.Internal))
        {
            return Task.FromResult(new List<TestGroup>());
        }

        // Skip if only externalMu is supported
        if (!parameters.ExternalMu.Contains(false))
        {
            return Task.FromResult(new List<TestGroup>());
        }

        // Skip if deterministic signatures are not included
        if (!parameters.Deterministic.Contains(true))
        {
            return Task.FromResult(new List<TestGroup>());
        }
        
        foreach (var capability in parameters.Capabilities)
        {
            // Skip if message length 256 is not supported
            if (!capability.MessageLength.IsWithinDomain(256))
            {
                continue;
            }

            foreach (var parameterSet in capability.ParameterSets)
            {
                testGroups.Add(new TestGroup
                {
                    TestType = "AFT",
                    ParameterSet = parameterSet,
                    Deterministic = true,
                    MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    SignatureInterface = SignatureInterface.Internal,
                    ExternalMu = false,
                    CornerCase = MLDSASignatureCornerCase.TotalRejection
                });
                
                testGroups.Add(new TestGroup
                {
                    TestType = "AFT",
                    ParameterSet = parameterSet,
                    Deterministic = true,
                    MessageLength = new MathDomain().AddSegment(new ValueDomainSegment(256)),
                    SignatureInterface = SignatureInterface.Internal,
                    ExternalMu = false,
                    CornerCase = MLDSASignatureCornerCase.AllRejectionCheck
                });
            }
        }
        
        return Task.FromResult(testGroups);
    }
}
