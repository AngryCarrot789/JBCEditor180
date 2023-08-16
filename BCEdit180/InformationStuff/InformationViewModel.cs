using System.Collections.ObjectModel;
using System.Windows.Input;
using BCEdit180.Core;

namespace BCEdit180.InformationStuff {
    public class InformationViewModel : BaseViewModel {
        public ObservableCollection<InformationModel> InformationItems { get; set; }

        public ICommand ClearInfoItemsCommand { get; }

        public InformationViewModel() {
            this.InformationItems = new ObservableCollection<InformationModel>();

            this.ClearInfoItemsCommand = new RelayCommand(this.Clear);
        }

        public void AddInformation(InformationModel information) {
            this.InformationItems.Insert(0, information);
        }

        public void RemoveInformation(InformationModel information) {
            this.InformationItems.Remove(information);
        }

        public void Clear() {
            this.InformationItems.Clear();
        }
    }
}