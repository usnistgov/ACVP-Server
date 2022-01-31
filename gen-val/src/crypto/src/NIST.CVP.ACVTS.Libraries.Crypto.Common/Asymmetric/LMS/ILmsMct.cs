using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public interface ILmsMct
    {
        Task<MCTResult<AlgoArrayResponse>> MCTHashAsync(LmsType[] lmsTypes, LmotsType[] lmotsTypes,
            BitString seed, BitString rootI, BitString message, bool isSample);
    }
}
