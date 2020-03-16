namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IJsonProvider
    {
        string GetJson(int vsId, string jsonFileType);
        void PutJson(int vsId, string jsonFileType, string json);
    }
}