namespace GoldDiff.OperatingSystem
{
    public class ProcessEventEventArguments
    {
        public int ProcessId { get; }

        public ProcessEventEventArguments(int processId)
        {
            ProcessId = processId;
        }
    }
}