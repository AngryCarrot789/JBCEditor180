using System.Threading.Tasks;

namespace BCEdit180.Core.Drop {
    public interface IDropHandler {
        /// <summary>
        /// Called when a drag drop enters the UI
        /// </summary>
        /// <param name="paths">The files in the drag drop. Will contain 1 or more files</param>
        /// <returns>The output drop type</returns>
        DropType OnDropEnter(string[] paths);

        /// <summary>
        /// Called when the last drag drop left the UI or was cancelled
        /// </summary>
        void OnDropLeave(bool isCancelled);

        Task OnFilesDropped(string[] paths);
    }
}