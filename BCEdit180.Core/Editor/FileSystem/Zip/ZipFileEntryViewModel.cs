using System.IO;
using System.Threading.Tasks;

namespace BCEdit180.Core.Editor.FileSystem.Zip {
    public class ZipFileEntryViewModel : BaseZipItemViewModel {
        public ZipFileEntryViewModel(ZipFileViewModel ownerZip) : base(ownerZip) {

        }

        protected override async Task<bool> OnExpandAsync() {
            if (this.Explorer != null && Path.GetExtension(this.ZipFileName) == ".class") {
                await this.Explorer.OpenFileAsync(this);
            }

            return false;
        }
    }
}