using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BCEdit180.Utils {
    public class TextBoxLineNumbers {
        #region BindableLineCount AttachedProperty

        public static string GetBindableLineCount(DependencyObject obj) {
            return (string) obj.GetValue(BindableLineCountProperty);
        }

        public static void SetBindableLineCount(DependencyObject obj, string value) {
            obj.SetValue(BindableLineCountProperty, value);
        }

        // Using a DependencyProperty as the backing store for BindableLineCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindableLineCountProperty =
            DependencyProperty.RegisterAttached(
                "BindableLineCount",
                typeof(string),
                typeof(TextBoxLineNumbers),
                new UIPropertyMetadata("1"));

        #endregion // BindableLineCount AttachedProperty

        #region HasBindableLineCount AttachedProperty

        public static bool GetHasBindableLineCount(DependencyObject obj) {
            return (bool) obj.GetValue(HasBindableLineCountProperty);
        }

        public static void SetHasBindableLineCount(DependencyObject obj, bool value) {
            obj.SetValue(HasBindableLineCountProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasBindableLineCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasBindableLineCountProperty =
            DependencyProperty.RegisterAttached(
                "HasBindableLineCount",
                typeof(bool),
                typeof(TextBoxLineNumbers),
                new UIPropertyMetadata(
                    false,
                    new PropertyChangedCallback(OnHasBindableLineCountChanged)));

        private static bool ProcessLineCounter;

        private static void OnHasBindableLineCountChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) {
            if (o is TextBox textBox) {
                if ((e.NewValue as bool?) == true) {
                    ProcessLineCounter = true;
                    textBox.TextChanged += TextBox_TextChanged;
                    textBox.SetValue(BindableLineCountProperty, textBox.LineCount.ToString());
                }
                else {
                    ProcessLineCounter = false;
                    textBox.SetValue(BindableLineCountProperty, "");
                    textBox.TextChanged -= TextBox_TextChanged;
                }
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (ProcessLineCounter) {
                TextBox textBox = (TextBox) sender;
                string x = string.Empty;

                string lineCounter = (string) textBox.GetValue(BindableLineCountProperty);

                int lineCount = textBox.LineCount;
                //string[] lines = lineCounter.Split('\n');
                //Task.Run(() =>
                //{
                Task.Run(async () => {
                    for (int line = 0; line < lineCount; line++) {
                        x += line + 1 + "\n";
                    }

                    await Task.Delay(1);
                    WriteText(textBox, x);
                });
            }
        }

        private static void WriteText(TextBox tb, string text) {
            Application.Current.Dispatcher.Invoke(() => {
                tb.SetValue(BindableLineCountProperty, text);
            });
        }

        #endregion // HasBindableLineCount AttachedProperty
    }
}