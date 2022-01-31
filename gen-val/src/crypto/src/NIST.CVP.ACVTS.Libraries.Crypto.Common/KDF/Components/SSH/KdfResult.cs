namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH
{
    public class KdfResult
    {
        public OneWayResult ServerToClient { get; }
        public OneWayResult ClientToServer { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public KdfResult(OneWayResult serverToClient, OneWayResult clientToServer)
        {
            ServerToClient = serverToClient;
            ClientToServer = clientToServer;
        }

        public KdfResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
