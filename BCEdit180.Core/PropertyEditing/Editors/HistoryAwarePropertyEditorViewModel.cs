using System;
using BCEdit180.Core.History;
using BCEdit180.Core.History.ViewModels;

namespace BCEdit180.Core.PropertyEditing.Editors {
    public abstract class HistoryAwarePropertyEditorViewModel : BasePropertyEditorViewModel {
        protected HistoryManagerViewModel HistoryManager;

        protected HistoryAwarePropertyEditorViewModel(Type applicableType) : base(applicableType) {
        }

        protected override void OnHandlersLoaded() {
            base.OnHandlersLoaded();
            foreach (IHistoryHolder holder in this.Handlers) {
                if ((this.HistoryManager = holder.HistoryManager) != null) {
                    return;
                }
            }
        }

        protected override void OnClearHandlers() {
            base.OnClearHandlers();
            this.HistoryManager = null;
        }

        public bool IsChangingAny() {
            foreach (object handler in this.Handlers) {
                if (handler is IHistoryHolder holder && holder.IsHistoryChanging) {
                    return true;
                }
            }

            return false;
        }
    }
}