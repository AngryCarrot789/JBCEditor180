using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BCEdit180.Converters {
    public class TextToFontDecorationConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return null;

            switch (value.ToString()) {
                case "None": return null;
                case "Underline": return TextDecorations.Underline;
                case "Strikethrough": return TextDecorations.Strikethrough;
                case "OverLine": return TextDecorations.OverLine;
                case "Baseline": return TextDecorations.Baseline;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return "None";
        }
    }
}