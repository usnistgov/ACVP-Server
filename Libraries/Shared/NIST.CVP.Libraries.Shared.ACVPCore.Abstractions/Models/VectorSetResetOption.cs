namespace NIST.CVP.Libraries.Internal.ACVPCore.Models
{
	/// <summary>
	/// Enum used to provide options to reset a vector sets status so it can be resubmitted to the task queue.
	///
	/// Vector sets will be able to be reset in instances where generation or validation failed.
	/// </summary>
	public enum VectorSetResetOption
	{
		None,
		ResetToGenerate,
		ResetToValidate
	}
}