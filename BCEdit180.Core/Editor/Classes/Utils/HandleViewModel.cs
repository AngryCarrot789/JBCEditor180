using JavaAsm.Instructions.Types;

namespace BCEdit180.Core.Editor.Classes.Utils {
    /// <summary>
    /// A view model that encapsulated a handle
    /// </summary>
    public class HandleViewModel : BaseViewModel {
        private Handle handle;
        public Handle Handle {
            get => this.handle;
            set => this.RaisePropertyChanged(ref this.handle, value);
        }

        public HandleViewModel() {
        }
    }
}