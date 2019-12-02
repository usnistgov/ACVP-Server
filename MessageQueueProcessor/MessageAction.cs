namespace MessageQueueProcessor
{
	public enum MessageAction
	{
		Unknown,
		CreateDependency,
		UpdateDependency,
		DeleteDependency,
		CreateImplementation,
		UpdateImplementation,
		DeleteImplementation,
		CreateOE,
		UpdateOE,
		DeleteOE,
		CreatePerson,
		UpdatePerson,
		DeletePerson,
		CreateVendor,
		UpdateVendor,
		DeleteVendor,
		RegisterTestSession,
		CancelTestSession,
		CertifyTestSession,
		SubmitVectorSetResults,
		CancelVectorSet
	}
}
