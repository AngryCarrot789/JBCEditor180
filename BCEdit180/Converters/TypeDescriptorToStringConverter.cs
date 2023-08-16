using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using JavaAsm;

namespace BCEdit180.Converters {
    public class TypeDescriptorToStringConverter : IValueConverter {
        public static TypeDescriptorToStringConverter Instance { get; } = new TypeDescriptorToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || value == DependencyProperty.UnsetValue) {
                return value;
            }

            if (value is TypeDescriptor || value is MethodDescriptor) {
                return value.ToString();
            }

            return "DEBUG_ERROR_NOT_DESCRIPTOR: " + (value.GetType() + " -> " + value);
            // throw new Exception("Cannot convert type " + value.GetType() + " to a string");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string str = value?.ToString() ?? "";
            if (str.Length == 0) {
                return DependencyProperty.UnsetValue;
            }
            else if (str[0] == '(') {
                try {
                    return MethodDescriptor.Parse(str);
                }
                catch {
                    return DependencyProperty.UnsetValue;
                }
            }
            else {
                try {
                    return TypeDescriptor.Parse(str);
                }
                catch {
                    return DependencyProperty.UnsetValue;
                }
            }
        }
    }
}
