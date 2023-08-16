using System;
using System.Diagnostics;
using System.Windows;

namespace BCEdit180.CClipboard {
    public static class CustomClipboard {
        public static void SetTextObject(object obj) {
            try {
                Clipboard.SetData(DataFormats.Text, obj);
            }
            catch (Exception e) {
                Debug.WriteLine(e);
                return;
            }
        }

        public static object GetTextObject() {
            try {
                return Clipboard.GetDataObject()?.GetData(typeof(string));
            }
            catch (Exception e) {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}