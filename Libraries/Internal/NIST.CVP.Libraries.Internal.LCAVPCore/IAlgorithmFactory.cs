using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface IAlgorithmFactory
	{
		List<IAlgorithm> GetAlgorithms(InfAlgorithm infAlgorithm);
	}
}