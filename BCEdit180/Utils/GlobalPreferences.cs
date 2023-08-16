using System.Windows.Media;

namespace BCEdit180.Utils {
    public static class GlobalPreferences {
        public const int WINDOW_TITLEBAR_HEIGHT = 30;

        // a delay to add to functions that add notepad items 
        // during the startup (to allow properties/fonts/etc to load)
        public static int STARTUP_NOTEPAD_ACTIONS_DELAY_MS = 250;
        public static double ANIMATION_SPEED_SEC = 0.15;
        public static double NOTEPADLIST_ANIMATION_SPEED_SEC = 0.15;

        // 50mb... why would you try to load that :/
        public const double MAX_FILE_SIZE = 50000000.0d;
        public const double WARN_FILE_SIZE_BYTES = 100000.0d;
        public const double ALERT_FILE_SIZE_BYTES = 250000.0d;
        public static Color WARN_FILE_TOO_BIG_COLOUR = Colors.Orange;
        public static Color ALERT_FILE_TOO_BIG_COLOUR = Colors.Red;

        public static bool ENABLE_FILE_WATCHER = true;
        public static int FILEWATCHER_FILE_RENAME_DELAY_MS = 100;

        //this is big for no reason other than why not lol
        public const int MAX_FONT_SIZE = 250;

        public static string[] PRESET_EXTENSIONS = new string[14] {
            ".txt",
            ".text",
            ".cs",
            ".c",
            ".cpp",
            ".h",
            ".hpp",
            ".xaml",
            ".xml",
            ".htm",
            ".html",
            ".css",
            ".js",
            ".exe"
        };
    }
}