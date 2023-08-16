namespace BCEdit180.Core.History {
    public interface IHistoryManager {
        void AddAction(IHistoryAction action, string information = null);
    }
}