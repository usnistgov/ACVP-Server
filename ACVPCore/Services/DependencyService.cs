using ACVPCore.Providers;

namespace ACVPCore.Services
{
	public class DependencyService : IDependencyService
	{
		IDependencyProvider _dependencyProvider;

		public DependencyService(IDependencyProvider dependencyProvider)
		{
			_dependencyProvider = dependencyProvider;
		}

		public void Delete(long dependencyID)
		{
			//Delete all OE links to the dependency
			_dependencyProvider.DeleteAllOELinks(dependencyID);

			//Delete all attributes under the dependency
			_dependencyProvider.DeleteAllAttributes(dependencyID);

			//Delete the dependency
			_dependencyProvider.Delete(dependencyID);
		}
	}
}
