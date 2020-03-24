using ACVPCore.Results;

namespace Web.Public.Results
{
    public class VendorResult : Result
    {
        public string URL => $"url/{ID}";
        public int ID { get; }

        public VendorResult(int id)
        {
            ID = id;
        }

        public VendorResult(string errorMessage) : base(errorMessage) { }
    }
}