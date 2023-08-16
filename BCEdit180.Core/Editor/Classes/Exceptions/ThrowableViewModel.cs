using JavaAsm;

namespace BCEdit180.Core.Editor.Classes.Exceptions {
    public class ThrowableViewModel : ClassNameViewModel {
        public ThrowableViewModel() {
        }

        public ThrowableViewModel(string fullName) : base(fullName) {
        }

        public ThrowableViewModel(ClassName name) : base(name) {
        }
    }
}