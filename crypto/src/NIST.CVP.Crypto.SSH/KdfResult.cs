using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.SSH
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

    public class OneWayResult
    {
        public BitString InitialIv { get; set; }
        public BitString EncryptionKey { get; set; }
        public BitString IntegrityKey { get; set; }
    }
}
