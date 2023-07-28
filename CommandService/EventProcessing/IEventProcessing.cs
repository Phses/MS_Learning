namespace CommandService.EventProcessing
{
    public interface IEventProcessing
    {
        void EventProcess(string message);
    }
}
