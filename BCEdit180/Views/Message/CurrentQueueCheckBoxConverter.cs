using System;
using System.Globalization;
using System.Windows.Data;
using BCEdit180.Converters;

namespace BCEdit180.Views.Message {
    public class CurrentQueueCheckBoxConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values == null || values.Length != 3) {
                throw new Exception("Expected 3 elements, not " + (values != null ? values.Length.ToString() : "null"));
            }

            bool a = (bool) values[0]; // showOptionAlwaysUseResult
            bool b = (bool) values[1]; // isAlwaysUseNextResult
            bool c = (bool) values[2]; // showOptionAlwaysUseResultForCurrent
            if (a && b && c) {
                return NullToVisibilityConverter._VisibleBox;
            }

            return NullToVisibilityConverter._CollapsedBox;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}