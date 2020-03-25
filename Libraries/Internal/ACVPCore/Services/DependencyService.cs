using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;


namespace ACVPCore.Services
{
	public class DependencyService : IDependencyService
	{
		IDependencyProvider _dependencyProvider;

		public DependencyService(IDependencyProvider dependencyProvider)
		{
			_dependencyProvider = dependencyProvider;
		}

		public InsertResult InsertAttribute(long dependencyID, string name, string value)
		{
			return _dependencyProvider.InsertAttribute(dependencyID, name, value);
		}

		public Result DeleteAttribute(long attributeID)
		{
			return _dependencyProvider.DeleteAttribute(attributeID);
		}

		public DeleteResult Delete(long dependencyID)
		{
			Result result;

			//Check to see if the dependency is used, in which case it can't be deleted
			if (DependencyIsUsed(dependencyID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete all attributes under the dependency
			result = _dependencyProvider.DeleteAllAttributes(dependencyID);

			if (!result.IsSuccess) new DeleteResult(result);

			//Delete the dependency
			result = _dependencyProvider.Delete(dependencyID);

			return new DeleteResult(result);
		}

		public DeleteResult DeleteEvenIfUsed(long dependencyID)
		{
			//This will delete a dependency even if it is in use, cleaning up any OE links that may exist

			Result result;

			//Delete all OE links to the dependency
			result = _dependencyProvider.DeleteAllOELinks(dependencyID);

			if (!result.IsSuccess) return new DeleteResult(result);

			//Delete all attributes under the dependency
			result = _dependencyProvider.DeleteAllAttributes(dependencyID);

			if (!result.IsSuccess) return new DeleteResult(result);

			//Delete the dependency
			result = _dependencyProvider.Delete(dependencyID);

			return new DeleteResult(result);
		}

		public DependencyResult Create(DependencyCreateParameters dependency)
		{
			//Insert the dependency record
			InsertResult dependencyInsertResult = _dependencyProvider.Insert(dependency.Type, dependency.Name, dependency.Description);

			if (!dependencyInsertResult.IsSuccess)
			{
				return new DependencyResult(dependencyInsertResult.ErrorMessage);
			}

			//Insert all the attribute records
			InsertResult attributeResult;
			foreach (DependencyAttributeCreateParameters attribute in dependency.Attributes)
			{
				attributeResult = _dependencyProvider.InsertAttribute(dependencyInsertResult.ID, attribute.Name, attribute.Value);
			}

			return new DependencyResult(dependencyInsertResult.ID);
		}

		public DependencyResult Update(DependencyUpdateParameters parameters)
		{
			//Update the dependency record if needed
			if (parameters.TypeUpdated || parameters.NameUpdated || parameters.DescriptionUpdated)
			{
				Result dependencyUpdateResult = _dependencyProvider.Update(parameters.ID, parameters.Type, parameters.Name, parameters.Description, parameters.TypeUpdated, parameters.NameUpdated, parameters.DescriptionUpdated);

				if (!dependencyUpdateResult.IsSuccess)
				{
					return new DependencyResult(dependencyUpdateResult.ErrorMessage);
				}
			}

			//Do the attribute update if needed. This is a full replacement, no changing of individual attributes since they are never referenced externally by the IDs
			if (parameters.AttributesUpdated)
			{
				//Get the current attributes
				List<DependencyAttribute> currentAttributes = _dependencyProvider.GetAttributes(parameters.ID);

				//Delete all current attributes that are not in the new collection
				foreach (DependencyAttribute attribute in currentAttributes.Where(x => !parameters.Attributes.Exists(p => p.Name == x.Name && p.Value == x.Value)))
				{
					_dependencyProvider.DeleteAttribute(attribute.ID);
				}

				//Add all attributes in the new collection that are not in the current collection
				foreach (var foo in parameters.Attributes.Where(p => !currentAttributes.Exists(c => c.Name == p.Name && c.Value == p.Value)))
				{
					_dependencyProvider.InsertAttribute(parameters.ID, foo.Name, foo.Value);
				}
			}

			//Even though it is kind of stupid, return a result object that includes the URL, as I think that's what is expected to go into the workflow item
			return new DependencyResult(parameters.ID);
		}

		public Dependency Get(long dependencyId)
		{
			return _dependencyProvider.Get(dependencyId);
		}

		public PagedEnumerable<Dependency> Get(DependencyListParameters param)
		{
			return _dependencyProvider.Get(param);
		}

		public bool DependencyIsUsed(long dependencyID)
		{
			return _dependencyProvider.DependencyIsUsed(dependencyID);
		}

		public bool DependencyExists(long dependencyID)
		{
			return _dependencyProvider.DependencyExists(dependencyID);
		}
	}
}
