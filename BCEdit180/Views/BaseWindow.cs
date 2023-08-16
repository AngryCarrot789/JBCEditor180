using System.Threading.Tasks;
using BCEdit180.Core.Views.Windows;

namespace BCEdit180.Views {
    public class BaseWindow : WindowViewBase, IWindow {
        public bool IsOpen => base.IsLoaded;

        public BaseWindow() {
            this.SetToCenterOfScreen();
        }

        public void CloseWindow() {
            this.Close();
        }

        public Task CloseWindowAsync() {
            return base.CloseAsync();
        }
    }
}