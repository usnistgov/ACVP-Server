using System.Collections.Generic;
using ACVPCore.Models.Parameters;
using ACVPCore.Results;
using ACVPCore.Services;
using LCAVPCore.Registration;

namespace LCAVPCore.Processors
{
	public class OEProcessor : IOEProcessor
	{
		private readonly IOEService _oeService;
		private readonly IDependencyService _dependencyService;

		public OEProcessor(IOEService oeService, IDependencyService dependencyService)
		{
			_oeService = oeService;
			_dependencyService = dependencyService;
		}

		public InsertResult Create(OperationalEnvironment oe)
		{
			List<long> dependencyIDs = new List<long>();

			foreach (IDependency dependency in oe.Dependencies)
			{
				DependencyCreateParameters dependencyCreateParameters = new DependencyCreateParameters
				{
					Name = dependency.Name,
					Type = dependency.Type,
					Description = dependency.Description,
					Attributes = new List<DependencyAttributeCreateParameters>()
				};

				//From CAVS only a processor dependency may result in an attribute, and that's only if the manufacturer is among the small number I chose to parse out
				if (dependency is ProcessorDependency && !string.IsNullOrEmpty(((ProcessorDependency)dependency).Manufacturer))
				{
					dependencyCreateParameters.Attributes.Add(new DependencyAttributeCreateParameters
					{
						Name = "manufacturer",
						Value = ((ProcessorDependency)dependency).Manufacturer
					});
				}

				var dependencyResult = _dependencyService.Create(dependencyCreateParameters);

				if (dependencyResult.IsSuccess)
				{
					dependencyIDs.Add(dependencyResult.ID);
				}
			}

			OECreateParameters createParameters = new OECreateParameters
			{
				Name = oe.Name,
				DependencyIDs = dependencyIDs
			};

			return _oeService.Create(createParameters);

		}

		public void Update(OperationalEnvironment oe)
		{
			//Because of the weird way we go from the CAVS info to dependencies, we just assume all new dependencies. Not even going to bother looking for new ones. At most there are 2.
			List<long> dependencyIDs = new List<long>();

			foreach (IDependency dependency in oe.Dependencies)
			{
				DependencyCreateParameters dependencyCreateParameters = new DependencyCreateParameters
				{
					Name = dependency.Name,
					Type = dependency.Type,
					Description = dependency.Description,
					Attributes = new List<DependencyAttributeCreateParameters>()
				};

				if (dependency is ProcessorDependency && !string.IsNullOrEmpty(((ProcessorDependency)dependency).Manufacturer))
				{
					dependencyCreateParameters.Attributes.Add(new DependencyAttributeCreateParameters
					{
						Name = "manufacturer",
						Value = ((ProcessorDependency)dependency).Manufacturer
					});
				}

				var dependencyResult = _dependencyService.Create(dependencyCreateParameters);

				if (dependencyResult.IsSuccess)
				{
					dependencyIDs.Add(dependencyResult.ID);
				}
			}

			OEUpdateParameters updateParameters = new OEUpdateParameters
			{
				ID = oe.ID,
				Name = oe.Name,
				NameUpdated = true,
				DependencyIDs = dependencyIDs,
				DependenciesUpdated = true
			};

			_oeService.Update(updateParameters);
		}
	}
}
