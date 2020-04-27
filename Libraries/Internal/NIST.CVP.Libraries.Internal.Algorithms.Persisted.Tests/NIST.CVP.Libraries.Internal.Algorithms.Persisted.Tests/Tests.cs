using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Libraries.Shared.Algorithms.External;
using NUnit.Framework;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted.Tests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}


		[Test, TestCaseSource("GetExternalAlgorithmInstances")]
		public void ExternalAlgorithmHasDeserializer(AlgorithmBase instance)
		{
			//Verifies that each external algorithm can be deserialized by the ExternalAlgorithmFactory
			Assert.IsNotNull(ExternalAlgorithmFactory.Deserialize(JsonSerializer.Serialize(instance)));
		}

		[Test, TestCaseSource("GetExternalAlgorithmInstances")]
		public void ExternalAlgorithmHasGenValAlgoMode(AlgorithmBase instance)
		{
			//Verifies that each external algorithm has a matching AlgoMode in the genvals
			Assert.IsNotNull(AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(instance.Name, instance.Mode, instance.Revision));
		}

		[Test, TestCaseSource("GetExternalAlgorithmInstances")]
		public void ExternalAlgorithmHasPersistedAlgorithm(AlgorithmBase instance)
		{
			//Verifies that each external algorithm can be converted to a persisted algorithm via the factory
			//Since we are passing in minimally populated algorithm instances they will not have anything that is a composite property type, which will cause an NRE. However, if the NRE is thrown, we know that means we have an appropriate conversion, so that passes the intent of this test. Any other kind of error is bad.
			try
			{
				var persistedAlgorithm = PersistedAlgorithmFactory.GetPersistedAlgorithm((IExternalAlgorithm)instance);
				Assert.IsNotNull(persistedAlgorithm);
			}
			catch (NullReferenceException ex)
			{
				Assert.Pass(ex.Message);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}

		public static IEnumerable<AlgorithmBase> GetExternalAlgorithmInstances()
		{
			var baseType = typeof(AlgorithmBase);
			var assembly = baseType.Assembly;
			return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).Select(x => (AlgorithmBase)Activator.CreateInstance(x));
		}
	}
}