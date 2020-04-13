using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Generation.Core;
using Web.Public.Models;

namespace Web.Public.Services
{
	public class ParameterValidatorService : IParameterValidatorService
	{
		private readonly IGenValInvoker _genValInvoker;
		private readonly JsonSerializerOptions _jsonSerializerOptions;

		public ParameterValidatorService(IGenValInvoker genValInvoker)
		{
			_genValInvoker = genValInvoker;
			
			_jsonSerializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				IgnoreNullValues = true
			};
            
			// This needs to be set via constructor as Converters only has a getter
			_jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		}
		
		public ParameterValidationResult Validate(TestSessionRegistration registration)
		{
			var errors = new Dictionary<string, List<string>>();
			
			for (var i = 0; i < registration.Algorithms.Count; i++)
			{
				var currentAlgo = registration.Algorithms[i];
				var json = JsonSerializer.Serialize(currentAlgo, _jsonSerializerOptions);
				var result =
					_genValInvoker.CheckParameters(new ParameterCheckRequest(json));

				if (!result.Success)
				{
					errors.Add($"{currentAlgo.Algorithm}{(string.IsNullOrEmpty(currentAlgo.Mode) ? string.Empty : "-" + currentAlgo.Mode)}-{currentAlgo.Revision} - index {i}", result.ErrorMessage);
				}
			}
			
			return new ParameterValidationResult(errors);
		}
	}
}