using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public interface IDependencyService
	{
		DeleteResult Delete(long dependencyID);
		DeleteResult DeleteEvenIfUsed(long dependencyID);
		DependencyResult Create(DependencyCreateParameters dependency);
		DependencyResult Update(DependencyUpdateParameters parameters);
		Dependency Get(long dependencyId);
	}
}