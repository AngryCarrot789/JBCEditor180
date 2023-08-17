using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Zip;

namespace BCEdit180.Core.Editor.FileSystem.Events {
    public delegate Task OpenZipEntryEventHandler(ZipFileEntryViewModel file);
}