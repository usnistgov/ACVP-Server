namespace NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters
{
    public class AcvpUserCreateParameters
    {
        public PersonCreateParameters Person { get; set; }
        public byte[] Certificate { get; set; }
    }
}
