using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BCEdit180.Utils;

namespace BCEdit180.Converters {
    public class FileSizeToColourConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (double.TryParse(value.ToString(), out double fileSizeKB)) {
                if (fileSizeKB < GlobalPreferences.WARN_FILE_SIZE_BYTES)
                    return Colors.Transparent;
                else if (
                    fileSizeKB > GlobalPreferences.WARN_FILE_SIZE_BYTES &&
                    fileSizeKB < GlobalPreferences.ALERT_FILE_SIZE_BYTES)
                    return GlobalPreferences.WARN_FILE_TOO_BIG_COLOUR;
                else if (fileSizeKB > GlobalPreferences.ALERT_FILE_SIZE_BYTES)
                    return GlobalPreferences.ALERT_FILE_TOO_BIG_COLOUR;
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return (Color) value == Colors.Red ? GlobalPreferences.WARN_FILE_SIZE_BYTES : 0;
        }
    }
}