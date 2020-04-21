using NIST.CVP.Libraries.Internal.LCAVPCore.AlgorithmEvaluators;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface IAlgorithmEvaluatorFactory
	{
		IAlgorithmEvaluator GetEvaluator(InfAlgorithm algorithm, string submissionPath);
	}
}