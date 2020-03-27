using LCAVPCore.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace LCAVPCore
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection InjectLCAVPCore(this IServiceCollection services)
		{
			services.AddSingleton<IModuleProcessor, ModuleProcessor>();
			services.AddSingleton<IOEProcessor, OEProcessor>();
			services.AddSingleton<IOrganizationProcessor, OrganizationProcessor>();
			services.AddSingleton<IPersonProcessor, PersonProcessor>();
			services.AddSingleton<IValidationProcessor, ValidationProcessor>();

			services.AddSingleton<ISubmissionLogger, SubmissionLogger>();
			services.AddSingleton<INewSubmissionProcessor, NewSubmissionProcessor>();
			services.AddSingleton<IChangeSubmissionProcessor, ChangeSubmissionProcessor>();
			services.AddSingleton<IUpdateSubmissionProcessor, UpdateSubmissionProcessor>();
			services.AddSingleton<ILCAVPSubmissionProcessor, LCAVPSubmissionProcessor>();

			services.AddSingleton<IDataProvider, DataProvider>();
			services.AddSingleton<IAlgorithmFactory, AlgorithmFactory>();
			services.AddSingleton<IAlgorithmEvaluatorFactory, AlgorithmEvaluatorFactory>();
			services.AddSingleton<IInfFileParser, InfFileParser>();
			services.AddSingleton<IAlgorithmChunkParserFactory, AlgorithmChunkParserFactory>();

			return services;
		}
	}
}
