using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Internal.ACVPCore.Providers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.ACVPCore.Services
{
	public class OEService : IOEService
	{
		private readonly IOEProvider _oeProvider;

		public OEService(IOEProvider oeProvider)
		{
			_oeProvider = oeProvider;
		}

		public DeleteResult Delete(long oeID)
		{
			Result result;

			//Check to see if the dependency is used, in which case it can't be deleted
			if (OEIsUsed(oeID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete the dependency links, but don't delete the dependencies - they might be used/useful elsewhere
			result = _oeProvider.DeleteAllDependencyLinks(oeID);

			if (!result.IsSuccess) new DeleteResult(result);

			//Delete the OE
			result = _oeProvider.Delete(oeID);

			return new DeleteResult(result);
		}

		public OEResult Create(OECreateParameters oe)
		{
			//Insert the dependency record
			InsertResult oeInsertResult = _oeProvider.Insert(oe.Name);

			if (!oeInsertResult.IsSuccess)
			{
				return new OEResult(oeInsertResult.ErrorMessage);
			}

			//Insert all the dependency links)
			Result linkResult;
			foreach (long dependencyID in oe.DependencyIDs ?? new List<long>())
			{
				linkResult = _oeProvider.InsertDependencyLink(oeInsertResult.ID, dependencyID);
			}

			return new OEResult(oeInsertResult.ID);
		}

		public OEResult Update(OEUpdateParameters parameters)
		{
			//Update the dependency record if needed
			if (parameters.NameUpdated)
			{
				Result oeUpdateResult = _oeProvider.Update(parameters.ID, parameters.Name);

				if (!oeUpdateResult.IsSuccess)
				{
					return new OEResult(oeUpdateResult.ErrorMessage);
				}
			}

			//Do the dependencies update if needed. This is a full replacement, no changing of individual dependencies
			if (parameters.DependenciesUpdated)
			{
				//Get the current linked dependencies
				List<long> currentDependencies = _oeProvider.GetDependencyLinks(parameters.ID);

				//Delete all current dependency links that are not in the new collection
				foreach (long dependencyID in currentDependencies.Except(parameters.DependencyIDs))
				{
					_oeProvider.DeleteDependencyLink(parameters.ID, dependencyID);
				}

				//Add all dependency links in the new collection that are not in the current collection
				foreach (long dependencyID in parameters.DependencyIDs.Except(currentDependencies))
				{
					_oeProvider.InsertDependencyLink(parameters.ID, dependencyID);
				}
			}

			//Even though it is kind of stupid, return a result object that includes the URL, as I think that's what is expected to go into the workflow item
			return new OEResult(parameters.ID);
		}

		public bool OEIsUsed(long oeID)
		{
			return _oeProvider.OEIsUsed(oeID);
		}

		public bool OEExists(long oeID)
		{
			return _oeProvider.OEExists(oeID);
		}

		public OperatingEnvironment Get(long oeID)
		{
			return _oeProvider.Get(oeID);
		}

		public PagedEnumerable<OperatingEnvironmentLite> Get(OeListParameters param)
		{
			return _oeProvider.Get(param);
		}

		public Result AddDependencyLink(long oeID, long dependencyID)
		{
			return _oeProvider.InsertDependencyLink(oeID, dependencyID);
		}

		public Result RemoveDependencyLink(long oeID, long dependencyID)
		{
			return _oeProvider.DeleteDependencyLink(oeID, dependencyID);
		}
	}
}
