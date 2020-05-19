namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IJsonProvider
    {
        string GetJson(long vsId, string jsonFileType);
        void PutJson(long vsId, string jsonFileType, string json);
    }
}