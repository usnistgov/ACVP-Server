using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface IChangeSubmissionProcessor
	{
		List<ProcessingResult> Process(string submissionRoot);
		List<(OperationalEnvironment OperationalEnvironment, string ErrorMessage)> BuildOEUpdate(ChangeFile changeFile);
		(Vendor Vendor, string ErrorMessage) BuildVendorUpdate(ChangeFile changeFile);
		(Module Module, string ErrorMessage) BuildModuleUpdate(ChangeFile changeFile);
		(Contact Contact, List<string> ErrorMessages) BuildContactUpdate(ChangeFile changeFile, int orderIndex);
	}
}