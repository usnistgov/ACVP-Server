using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.MLKEM;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<TestVectorSet, TestGroup, TestCase>
{
    public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(TestVectorSet testVectorSet)
    {
        var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

        foreach (var group in testVectorSet.TestGroups.Select(g => g))
        {
            foreach (var test in group.Tests.Select(t => t))
            {
                list.Add(group.Function switch
                {
                    MLKEMFunction.Encapsulation => new TestCaseValidatorEncapsulationAft(test),
                    MLKEMFunction.Decapsulation => new TestCaseValidatorDecapsulationVal(test),
                    MLKEMFunction.EncapsulationKeyCheck => new TestCaseValidatorKeyCheck(test),
                    MLKEMFunction.DecapsulationKeyCheck => new TestCaseValidatorKeyCheck(test),
                    
                    _ => throw new ArgumentOutOfRangeException()
                });
            }
        }

        return list;
    }
}
