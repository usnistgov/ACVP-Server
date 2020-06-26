using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Exceptions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Web.Public.Models;

namespace Web.Public.Services
{
	public class ParameterValidatorService : IParameterValidatorService
	{
		private readonly IGenValInvoker _genValInvoker;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly IAlgorithmService _algorithmService;

		public ParameterValidatorService(IGenValInvoker genValInvoker, IAlgorithmService algorithmService)
		{
			_genValInvoker = genValInvoker;
			_algorithmService = algorithmService;
			
			_jsonSerializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				IgnoreNullValues = true
			};
            
			// This needs to be set via constructor as Converters only has a getter
			_jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		}
		
		public ParameterValidationResult Validate(TestSessionRegisterPayload registration)
		{
			var errors = new List<string>();
			
			if (registration.Algorithms == null)
			{
				errors.Add("Algorithms property is required but was not provided.");
				return new ParameterValidationResult(errors);
			}

			if (registration.Algorithms.Count == 0)
			{
				errors.Add("No algorithms were present within the algorithms property.");
			}
			
			for (var i = 0; i < registration.Algorithms.Count; i++)
			{
				var currentAlgo = registration.Algorithms[i];
				var algoModeIndexConcat = $"{currentAlgo.Algorithm}{(string.IsNullOrEmpty(currentAlgo.Mode) ? string.Empty : "-" + currentAlgo.Mode)}-{currentAlgo.Revision} - index {i}";
				var algoObject = _algorithmService.GetAlgorithm(currentAlgo.Algorithm, currentAlgo.Mode, currentAlgo.Revision);
				
				if (algoObject == null)
				{
					errors.Add($"Unable to map {algoModeIndexConcat} to an internal algorithm id.");
					continue;
				}
				
				var json = JsonSerializer.Serialize(currentAlgo, _jsonSerializerOptions);

				ParameterCheckResponse result = null;
				try
				{
					result =
						_genValInvoker.CheckParameters(new ParameterCheckRequest(json));
				}
				catch (AlgoModeRevisionException e)
				{
					errors.Add($"Error on {algoModeIndexConcat}: {e.Message}");
					continue;
				}
				
				if (!result.Success)
				{
					errors.AddRange(result.ErrorMessage.Select(error => $"{algoObject.FullAlgoName}: {error}"));
				}
			}
			
			return new ParameterValidationResult(errors);
		}
	}
}