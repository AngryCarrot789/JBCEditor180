using System.ComponentModel;

namespace BCEdit180.Core.PropertyEditing {
    public interface IPropertyEditReceiver : INotifyPropertyChanged {
        void OnExternalPropertyModified(BasePropertyEditorViewModel handler, string property);
    }
}