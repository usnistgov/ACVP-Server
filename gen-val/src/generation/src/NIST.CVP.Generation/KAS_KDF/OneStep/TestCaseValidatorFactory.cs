using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_KDF.OneStep
{
	public class TestCaseValidatorFactory : ITestCaseValidatorFactoryAsync<VectorSet, TestGroup, TestCase>
	{
		public List<ITestCaseValidatorAsync<TestGroup, TestCase>> GetValidators(VectorSet testVectorSet)
		{
			var list = new List<ITestCaseValidatorAsync<TestGroup, TestCase>>();

			foreach (var group in testVectorSet.TestGroups.Select(g => g))
			{
				foreach (var test in group.Tests.Select(t => t))
				{
					var workingTest = test;

					if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(new TestCaseValidatorAft(workingTest));
					}
					else
					{
						list.Add(new TestCaseValidatorVal(workingTest));
					}
				}
			}

			return list;
		}
	}
}