using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Generation.Core;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
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
		
		public ParameterValidationResult Validate(TestSessionRegisterPayload registration)
		{
			var errors = new List<string>();
			
			for (var i = 0; i < registration.Algorithms.Count; i++)
			{
				var currentAlgo = registration.Algorithms[i];
				var json = JsonSerializer.Serialize(currentAlgo, _jsonSerializerOptions);
				var result =
					_genValInvoker.CheckParameters(new ParameterCheckRequest(json));

				if (!result.Success)
				{
					var algoMode = $"{currentAlgo.Algorithm}{(string.IsNullOrEmpty(currentAlgo.Mode) ? string.Empty : "-" + currentAlgo.Mode)}-{currentAlgo.Revision} - index {i}";
					errors.AddRange(result.ErrorMessage.Select(error => $"{algoMode}: {error}"));
				}
			}
			
			return new ParameterValidationResult(errors);
		}
	}
}