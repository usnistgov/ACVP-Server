namespace ACVPCore.Models.Parameters
{
    public class ImplementationListParameters : PagedParametersBase
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}