namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions
{
	public enum APIAction
	{
		Unknown = 0,
		CreateDependency = 1,
		UpdateDependency = 2,
		DeleteDependency = 3,
		CreateImplementation = 4,
		UpdateImplementation = 5,
		DeleteImplementation = 6,
		CreateOE = 7,
		UpdateOE = 8,
		DeleteOE = 9,
		CreatePerson = 10,
		UpdatePerson = 11,
		DeletePerson = 12,
		CreateVendor = 13,
		UpdateVendor = 14,
		DeleteVendor = 15,
		RegisterTestSession = 16,
		CancelTestSession = 17,
		CertifyTestSession = 18,
		SubmitVectorSetResults = 19,
		CancelVectorSet = 20,
        ResubmitVectorSetResults = 21,
		TestSessionKeepAlive = 22
	}
}
