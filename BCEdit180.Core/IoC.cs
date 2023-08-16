using System;
using BCEdit180.Core.Editor.Classes.Editors;
using BCEdit180.Core.Services;
using BCEdit180.Core.Shortcuts.Dialogs;
using BCEdit180.Core.Views.Dialogs.FilePicking;
using BCEdit180.Core.Views.Dialogs.Message;
using BCEdit180.Core.Views.Dialogs.Progression;
using BCEdit180.Core.Views.Dialogs.UserInputs;

namespace BCEdit180.Core {
    public static class IoC {
        private static volatile bool isAppRunning = true;

        public static SimpleIoC Instance { get; } = new SimpleIoC();

        public static bool IsAppRunning {
            get => isAppRunning;
            set => isAppRunning = value;
        }

        public static IShortcutManagerDialogService ShortcutManagerDialog => Provide<IShortcutManagerDialogService>();
        public static Action<string> OnShortcutModified { get; set; }
        public static Action<string> BroadcastShortcutActivity { get; set; }

        /// <summary>
        /// The application dispatcher, used to execute actions on the main thread
        /// </summary>
        public static IDispatcher Dispatcher { get; set; }

        public static IClipboardService Clipboard => Provide<IClipboardService>();
        public static IMessageDialogService MessageDialogs => Provide<IMessageDialogService>();
        public static IProgressionDialogService ProgressionDialogs => Provide<IProgressionDialogService>();
        public static IFilePickDialogService FilePicker => Provide<IFilePickDialogService>();
        public static IUserInputDialogService UserInput => Provide<IUserInputDialogService>();
        public static IExplorerService ExplorerService => Provide<IExplorerService>();
        public static IKeyboardDialogService KeyboardDialogs => Provide<IKeyboardDialogService>();
        public static IMouseDialogService MouseDialogs => Provide<IMouseDialogService>();

        public static ITranslator Translator => Provide<ITranslator>();

        public static IAccessEditorService AccessEditors => Provide<IAccessEditorService>();
        public static IDescEditorService TypeDescEditors => Provide<IDescEditorService>();

        public static Action<string> BroadcastShortcutChanged { get; set; }

        public static T Provide<T>() => Instance.GetService<T>();
    }
}