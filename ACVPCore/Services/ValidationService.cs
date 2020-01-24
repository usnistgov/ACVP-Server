using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models.Capabilities;
using ACVPCore.Models.ProtoModels;
using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class ValidationService : IValidationService
	{
		IValidationProvider _validationProvider;
		IScenarioProvider _scenarioProvider;
		IScenarioOEProvider _scenarioOEProvider;
		IScenarioAlgorithmProvider _scenarioAlgorithmProvider;
		ICapabilityService _capabilityService;

		public ValidationService(IValidationProvider validationProvider, IScenarioProvider scenarioProvider, IScenarioOEProvider scenarioOEProvider, IScenarioAlgorithmProvider scenarioAlgorithmProvider, ICapabilityService capabilityService)
		{
			_validationProvider = validationProvider;
			_scenarioProvider = scenarioProvider;
			_scenarioOEProvider = scenarioOEProvider;
			_scenarioAlgorithmProvider = scenarioAlgorithmProvider;
			_capabilityService = capabilityService;
		}

		public InsertResult Create(ValidationSource validationSource, long implementationID, long oeID)
		{
			//Get what the validation number to assign to it
			long validationNumber = GetValidationNumber(validationSource);

			if (validationNumber == -1) return new InsertResult("Failed to get new validation number");

			//Create the validation record
			InsertResult validationInsertResult = _validationProvider.Insert(validationSource, validationNumber, implementationID);

			if (!validationInsertResult.IsSuccess) return validationInsertResult;

			long validationID = validationInsertResult.ID;

			//Create the scenario and everything beneath it
			Result scenarioResult = CreateScenario(validationID, oeID);

			//Replace the previous error message (which was none) with the scenario method's error message. If it is null, then all is good
			validationInsertResult.ErrorMessage = scenarioResult.ErrorMessage;

			return validationInsertResult;
		}

		public Result CreateScenario(long validationID, long oeID)
		{
			//Create the scenario record
			InsertResult scenarioCreateResult = AddScenarioToValidation(validationID);

			if (!scenarioCreateResult.IsSuccess) return scenarioCreateResult;

			long scenarioID = scenarioCreateResult.ID;

			//Create the scenario OE link
			Result OEResult = AddOEToScenario(scenarioID, oeID);

			if (!OEResult.IsSuccess) return (InsertResult)OEResult;

			//foreach algorithm...
			//Create the scenario algorithm
			//Add the capabilities
			//Create prereqs
		}

		public InsertResult AddScenarioToValidation(long validationID) => _scenarioProvider.Insert(validationID);

		public Result AddOEToScenario(long scenarioID, long oeID) => _scenarioOEProvider.Insert(scenarioID, oeID);

		public InsertResult AddScenarioAlgorithm(long scenarioID, long algorithmID) => _scenarioAlgorithmProvider.Insert(scenarioID, algorithmID);

		public InsertResult AddCapability(long scenarioAlgorithmID, ICapability capability)
		{

		}

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID) => _validationProvider.GetValidationsForImplementation(implementationID);

		//Get the most recently created ACVP validation ID, or 0 if none exists
		public long GetLatestACVPValidationForImplementation(long implementationID) => GetValidationsForImplementation(implementationID).Where(x => x.ValidationSource == 1).DefaultIfEmpty().Max(x => x.ValidationID);

		public long GetValidationNumber(ValidationSource validationSource) => validationSource switch
		{
			ValidationSource.ACVP => _validationProvider.GetNextACVPValidationNumber(),
			ValidationSource.LCAVP => _validationProvider.GetNextLCAVPValidationNumber(),
			_ => -1
		};

		public long FindMatchingScenario(long validationID, List<ScenarioAlgorithm> scenarioAlgorithms)
		{
			//Default to a garbage value
			long scenarioID = -1;

			//Create a lookup object grouping the incoming scenario algorithms by algorithm, so we can deal with having multiple instances of one algorithm
			var groupedIncomingAlgorithms = scenarioAlgorithms.ToLookup(x => x.AlgorithmID, x => x);		//Yes, this is just extracting the algorithmID from the scenario algorithm to use as the key, and the whole scenario algorithm is the "value"

			//Get list of existing scenarios
			List<long> existingScenarios = _scenarioProvider.GetScenarioIDsForValidation(validationID);

			foreach (long existingScenarioID in existingScenarios)
			{
				//Get the existing scenario algorithms on this scenario
				var existingScenarioAlgorithms = _scenarioAlgorithmProvider.GetScenarioAlgorithmsForScenario(existingScenarioID);

				//Create a lookup of those scenario algorithm IDs grouped by the algorithm ID (ToLookup does immediate execution, vs. deferred for GroupBy)
				var groupedExistingAlgorithms = existingScenarioAlgorithms.ToLookup(x => x.AlgorithmID, x => x.ScenarioAlgorithmID);

				//Do the lightweight check - need to have the same algorithms, and the same number of instances of each. This turns the two lookups into ordered lists of (algorithmID, count of instances) and returns whether or not they match. Have to order because there's no Linq function that doesn't care about order. And yes, it is horribly ugly and opaque, but it works and would take a lot of lines of code to do another way
				if (groupedIncomingAlgorithms.Select(x => (x.Key, x.Count())).OrderBy(x => x.Key).SequenceEqual(groupedExistingAlgorithms.Select(x => (x.Key, x.Count())).OrderBy(x => x.Key)))
				{
					//Don't have the same number of algorithms/instances so it clearly can't be a match, so move on to the next existing scenario
					continue;
				}

				//Have same number of algorithms/instances so now need to do the heavy check - do the contents of all the scenario algorithms in this existing scenario match what is incoming?
				//Since this is nasty nested looping that we need to break out of, need a variable to keep track of whether or not we've found a match. Will default to true, then set to false as soon as we know the incoming scenario doesn't match an existing scenario
				bool existingScenarioMatchesIncoming = true;

				foreach (IGrouping<long, ScenarioAlgorithm> algorithmGroup in groupedIncomingAlgorithms)
				{
					//These variables are just to make what follows a little easier to understand
					long algorithmID = algorithmGroup.Key;
					IEnumerable<ScenarioAlgorithm> incomingScenarioAlgorithmsForAlgorithm = algorithmGroup;		//This looks weird, but that's the weird way Lookup/Grouping works... While this object is an IGrouping<long, ScenarioAlgorithm>, it also is an IEnumerable<ScenarioAlgorithm> that represents the grouped items.

					//Matching logic varies depending on the number of instances of the algorithm. Most of the time there is only 1, and the logic is simple.
					if (incomingScenarioAlgorithmsForAlgorithm.Count() == 1)
					{
						//Only one instance of this algorithm, so do a simple match - Does the incoming scenario algorithm match the existing scenario algorithm
						if (!IsScenarioAlgorithmMatch(incomingScenarioAlgorithmsForAlgorithm.First(), groupedExistingAlgorithms[algorithmID].First()))		//The First() is used because both things are IEnumberables that we know in this case only have 1 member, and we want that one member
						{
							existingScenarioMatchesIncoming = false;
							break;		//Since this algorithm doesn't match, can stop checking this scenario, so break out of the algorithm checking loop
						}
					}
					else
					{
						//Check each of the incoming instances of this algorithm for a match in the existing ones. We already know the number matches
						foreach (ScenarioAlgorithm incomingScenarioAlgorithm in incomingScenarioAlgorithmsForAlgorithm)
						{
							//Check if any of the existing scenario algorithms match this incoming scenario algorithm by passing each of them to the matching function. If not, stop checking this algorithm by breaking out of the inner loop. This is somewhat wasteful because it means the existing scenario algorithms get loaded multiple times. Would be best, I suppose to get a collection of fully populated objects and then reuse those.
							if (!groupedExistingAlgorithms[algorithmID].Any(x => IsScenarioAlgorithmMatch(incomingScenarioAlgorithm, x)))
							{
								existingScenarioMatchesIncoming = false;
								break;
							}
						}

						//If this algorithm didn't match then break out of the algorithm checking loop too
						if (!existingScenarioMatchesIncoming) break;
					}
				}

				//If we went through all that scenario algorithm matching logic and our match variable is still true, then we have found a matching existing scenario. Set the output variable to the matching scenario ID and break out of the scenario loop
				if (existingScenarioMatchesIncoming)
				{
					scenarioID = existingScenarioID;
					break;
				}
			}

			return scenarioID;		// -1 if didn't find a match, some other value if we did
		}

		public bool IsScenarioAlgorithmMatch(ScenarioAlgorithm incomingScenarioAlgorithm, long existingScenarioAlgorithmID)
		{
			//Match the capabilities

			//Get the capabilities for the existing scenario algorithm ID
			var existingCapabilities = _capabilityService.GetCapabilitiesForComparison(existingScenarioAlgorithmID);

			//TODO - Need to turn these into an algorithm object of the proper type. Look at CSRC code maybe? This is the iterate through the root items, create new based on children, etc.

			//If there are any incoming capabilities for which there is not an existing capability then not a match. Done as an Any so it short circuits
			if (incomingScenarioAlgorithm.Capabilities.Any(x => !existingCapabilities.Exists(c => c.Equals(x)))){		//TODO - this isn't equals, needs the more complex check
				return false;
			}

			//Need to check the other direction too - there can be no capabilities on the existing scenario algorithm that doesn't have a match in the incoming capabilities
			if (existingCapabilities.Any(x => !incomingScenarioAlgorithm.Capabilities.Exists(c => c.Equals(x))))        //TODO - this isn't equals, needs the more complex check
			{
				return false;
			}


			//TODO - Match the prereqs - add this once we have something worthwhile for prereqs

			return true;
		}
	}
}
