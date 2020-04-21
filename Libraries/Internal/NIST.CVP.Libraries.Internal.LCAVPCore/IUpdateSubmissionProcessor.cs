using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface IUpdateSubmissionProcessor
	{
		List<AlgorithmPrerequisite> ExtractPrerequisites(List<IAlgorithm> algorithms);
		List<Contact> ParseContacts(InfFile infFile);
		Module ParseModule(InfFile infFile);
		OperationalEnvironment ParseOperationalEnvironment(InfFile infFile);
		Vendor ParseVendor(InfFile infFile);
		List<ProcessingResult> Process(string submissionRoot);
	}
}