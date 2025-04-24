using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
public partial interface IOracle
{
    public Task<AsconAead128Result> GetAsconEncryptCaseAsync(AsconAEAD128Parameters param);
    public Task<AsconAead128Result> GetAsconDecryptCaseAsync(AsconAEAD128Parameters param);
    public Task<AsconHashResult> GetAsconHash256CaseAsync(AsconHashParameters param);
    public Task<AsconHashResult> GetAsconXOF128CaseAsync(AsconHashParameters param);
    public Task<AsconHashResult> GetAsconCXOF128CaseAsync(AsconHashParameters param);
}
