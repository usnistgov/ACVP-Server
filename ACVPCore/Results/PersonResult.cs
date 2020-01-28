namespace ACVPCore.Results
{
	public class PersonResult : InsertResult
	{
		public string URL { get => $"/admin/validations/persons/{ID}"; }

		public PersonResult(long id) : base(id) { }

		public PersonResult(string errorMessage) : base(errorMessage) { }
	}
}
