﻿using ACVPCore.Providers;
using ACVPCore.Results;

namespace ACVPCore.Services
{
	public class PrerequisiteService : IPrerequisiteService
	{
		private readonly IPrerequisiteProvider _prerequisiteProvider;

		public PrerequisiteService(IPrerequisiteProvider PrerequisiteProvider)
		{
			_prerequisiteProvider = PrerequisiteProvider;
		}

		public Result DeleteAllForScenarioAlgorithm(long scenarioAlgorithmID) => _prerequisiteProvider.DeleteAllForScenarioAlgorithm(scenarioAlgorithmID);
	}
}