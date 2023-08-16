using System;
using System.Windows;

namespace BCEdit180.InformationStuff {
    public static class Information {
        public delegate void InformationEventArgs(InformationModel e);

        public static event InformationEventArgs InformationAdded;

        public static void Show(string text) {
            Application.Current?.Dispatcher?.Invoke(() => {
                InformationAdded?.Invoke(new InformationModel("Info", DateTime.Now, text));
            });
        }

        public static void Show(string text, string type) {
            Application.Current?.Dispatcher?.Invoke(() => {
                InformationAdded?.Invoke(new InformationModel(type, DateTime.Now, text));
                //MessageBox.Show(text);
            });
        }

        public static void Show(string text, InfoTypes type) {
            Application.Current?.Dispatcher?.Invoke(() => {
                InformationAdded?.Invoke(new InformationModel(type, DateTime.Now, text));
            });
        }
    }
}