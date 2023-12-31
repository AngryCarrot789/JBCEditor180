using System;
using System.Threading.Tasks;
using BCEdit180.Core.Utils;

namespace BCEdit180.Core.History {
    /// <summary>
    /// A history action that fires an event when it is undone, redone or removed. The methods are delegated to the other action
    /// </summary>
    public class EventHistoryAction : IHistoryAction {
        public delegate void UndoEventHandler(EventHistoryAction action);

        public delegate void RedoEventHandler(EventHistoryAction action);

        public delegate void RemovedEventHandler(EventHistoryAction action);

        public IHistoryAction Action { get; }

        public bool IsRemoved { get; set; }

        public event UndoEventHandler Undo;
        public event RedoEventHandler Redo;
        public event RemovedEventHandler Removed;

        public EventHistoryAction(IHistoryAction action) {
            this.Action = action;
        }

        public async Task UndoAsync() {
            using (ErrorList stack = new ErrorList()) {
                try {
                    await this.Action.UndoAsync();
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to undo action", e));
                }

                try {
                    this.Undo?.Invoke(this);
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to fire model's undo event", e));
                }
            }
        }

        public async Task RedoAsync() {
            using (ErrorList stack = new ErrorList()) {
                try {
                    await this.Action.RedoAsync();
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to redo action", e));
                }

                try {
                    this.Redo?.Invoke(this);
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to fire model's redo event", e));
                }
            }
        }

        public void OnRemoved() {
            this.IsRemoved = true;
            using (ErrorList stack = new ErrorList()) {
                try {
                    this.Action.OnRemoved();
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to remove action", e));
                }

                try {
                    this.Removed?.Invoke(this);
                }
                catch (Exception e) {
                    stack.Add(new Exception("Failed to fire model's removed event", e));
                }
            }
        }
    }
}