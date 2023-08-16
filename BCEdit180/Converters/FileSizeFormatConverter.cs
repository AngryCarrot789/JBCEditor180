using System;
using System.Globalization;
using System.Windows.Data;

namespace BCEdit180.Converters {
    public class FileSizeFormatConverter : IValueConverter {
        static readonly string[] SizeSuffixes = {"Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        static string SizeSuffix(long value, int decimalPlaces = 3) {
            if (decimalPlaces < 0)
                return "No decimals";
            if (value == 0) {
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
            }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int) Math.Log(value, 1024);
            decimal adjustedSize = (decimal) value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimalPlaces) >= 1000) {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is long size)
                return SizeSuffix(size);
            //return string.Format("{0:#,###0}", size);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return (long) 1;
        }
    }
}