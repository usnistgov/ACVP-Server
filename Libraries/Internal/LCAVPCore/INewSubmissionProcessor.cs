using System.Collections.Generic;
using LCAVPCore.Registration;
using LCAVPCore.Registration.Algorithms;

namespace LCAVPCore
{
	public interface INewSubmissionProcessor
	{
		List<AlgorithmPrerequisite> ExtractPrerequisites(List<IAlgorithm> algorithms);
		List<Contact> ParseContacts(InfFile infFile);
		Module ParseModule(InfFile infFile);
		OperationalEnvironment ParseOperationalEnvironment(InfFile infFile);
		Vendor ParseVendor(InfFile infFile);
		List<ProcessingResult> Process(string submissionRoot);
	}
}