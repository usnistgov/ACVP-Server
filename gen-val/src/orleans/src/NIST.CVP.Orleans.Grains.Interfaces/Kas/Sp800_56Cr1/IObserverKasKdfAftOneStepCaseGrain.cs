using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Cr1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Cr1;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Cr1
{
	public interface IObserverKasKdfAftOneStepCaseGrain : IGrainWithGuidKey, IGrainObservable<KasKdfAftOneStepResult>
	{
		Task<bool> BeginWorkAsync(KasKdfAftOneStepParameters param);
	}
}