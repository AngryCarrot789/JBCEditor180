using System;
using System.Globalization;
using System.IO;
using BCEdit180.Core.Utils;

namespace BCEdit180.Converters {
    public class FileExistsToBoolConverter : SingletonValueConverter<FileExistsToBoolConverter> {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return File.Exists((string) value).Box();
        }
    }
}