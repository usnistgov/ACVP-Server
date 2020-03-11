using LCAVPCore.AlgorithmEvaluators;

namespace LCAVPCore
{
	public interface IAlgorithmEvaluatorFactory
	{
		IAlgorithmEvaluator GetEvaluator(InfAlgorithm algorithm, string submissionPath);
	}
}