namespace NIST.CVP.TaskQueueProcessor
{
    public interface ITask
    {
        int DbId { get; set; }
        int VsId { get; set; }
        bool IsSample { get; set; }

        void Run();
    }
}