using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
{
	public class TestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase, TKeyPair> : ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase>
		where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase, TKeyPair>
		where TTestGroup : TestGroupBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TTestCase : TestCaseBase<TTestGroup, TTestCase, TKeyPair>, new()
		where TKeyPair : IDsaKeyPair
	{
		private IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult> _deferredTestCaseResolver;

		public TestCaseValidatorFactory(IDeferredTestCaseResolverAsync<TTestGroup, TTestCase, KeyAgreementResult> deferredTestCaseResolver)
		{
			_deferredTestCaseResolver = deferredTestCaseResolver;
		}
        
		public List<ITestCaseValidatorAsync<TTestGroup, TTestCase>> GetValidators(TTestVectorSet testVectorSet)
		{
			var list = new List<ITestCaseValidatorAsync<TTestGroup, TTestCase>>();

			foreach (var group in testVectorSet.TestGroups.Select(g => g))
			{
				foreach (var test in group.Tests.Select(t => t))
				{
					var workingTest = test;

					if (group.TestType.Equals("aft", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(new TestCaseValidatorAft<TTestGroup, TTestCase, TKeyPair>(
							workingTest,
							group,
							_deferredTestCaseResolver));
					}
					else
					{
						list.Add(new TestCaseValidatorVal<TTestGroup, TTestCase, TKeyPair>(workingTest));
					}
				}
			}

			return list;
		}
	}
}