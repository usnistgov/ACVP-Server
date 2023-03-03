using System.Threading.Tasks;
using Autofac;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.HSS.KeyGen.IntegrationTests.Helpers
{
	public static class OracleHelper
	{
		private static IOracle _oracle;
		
		public static async Task<IOracle> GetOracle()
		{
			if (_oracle != null)
				return _oracle;

			var container = GetContainer();
			var oracle = container.Resolve<IOracle>();

			await oracle.InitializeClusterClient();

			_oracle = oracle;
			return _oracle;
		}
		
		private static IContainer GetContainer()
		{
			var serviceProvider = EntryPointConfigHelper.GetServiceProviderFromConfigurationBuilder();
			var builder = new ContainerBuilder();

			EntryPointConfigHelper.RegisterConfigurationInjections(serviceProvider, builder);

			new RegisterInjections().RegisterTypes(builder, AlgoMode.HSS_KeyGen_v1_0);

			return builder.Build();
		}
	}
}
