using System.Threading.Tasks;
using BCEdit180.Core.Editor.FileSystem.Physical;

namespace BCEdit180.Core.Editor.FileSystem.Events {
    public delegate Task OpenFileEventHandler(IOFileItemViewModel file);
}