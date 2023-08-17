using System.IO;
using System.Threading.Tasks;

namespace BCEdit180.Core.Editor.FileSystem.Physical {
    /// <summary>
    /// A class for a regular file, such as a text document
    /// </summary>
    public class IOFileItemViewModel : BaseIOFileItemViewModel {
        public string Items { get; set; }

        public IOFileItemViewModel(string filePath) {
            this.FilePath = filePath;
        }

        protected override async Task<bool> OnExpandAsync() {
            if (this.Explorer != null && Path.GetExtension(this.FilePath) == ".class") {
                await this.Explorer.OpenFileAsync(this);
            }

            return false;
        }
    }
}