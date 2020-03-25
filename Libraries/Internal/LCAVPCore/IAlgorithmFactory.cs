using System.Collections.Generic;
using LCAVPCore.Registration.Algorithms;

namespace LCAVPCore
{
	public interface IAlgorithmFactory
	{
		List<IAlgorithm> GetAlgorithms(InfAlgorithm infAlgorithm);
	}
}