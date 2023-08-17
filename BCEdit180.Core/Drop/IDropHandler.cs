using System.Threading.Tasks;

namespace BCEdit180.Core.Drop {
    public interface IFileDropNotifier {
        Task<DropType> OnDropEnter(string[] paths);

        Task OnDropLeave();

        Task OnFilesDropped(string[] paths);
    }
}