using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using JavaAsm;

namespace BCEdit180.Converters {
    public class TypeDescriptorToReadableConverter : IValueConverter {
        public static string TypeDescToStr(TypeDescriptor desc) {
            if (desc.ClassName != null) {
                return desc.ClassName.Name.Replace('/', '.');
            }
            else if (!desc.PrimitiveType.HasValue) {
                return "void";
            }
            else {
                return PrimitveToString(desc.PrimitiveType.Value);
            }
        }

        public static string PrimitveToString(PrimitiveType type) {
            switch (type) {
                case PrimitiveType.Boolean:
                    return "boolean";
                case PrimitiveType.Byte:
                    return "byte";
                case PrimitiveType.Character:
                    return "char";
                case PrimitiveType.Double:
                    return "double";
                case PrimitiveType.Float:
                    return "float";
                case PrimitiveType.Integer:
                    return "int";
                case PrimitiveType.Long:
                    return "long";
                case PrimitiveType.Short:
                    return "short";
                case PrimitiveType.Void:
                    return "void";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || value == DependencyProperty.UnsetValue) {
                return value;
            }
            else switch (value) {
                case TypeDescriptor type:
                    return TypeDescToStr(type);
                case MethodDescriptor md: {
                    StringBuilder result = new StringBuilder();
                    result.Append('(');
                    for (int i = 0; i < (md.ArgumentTypes.Count - 1); i++) {
                        result.Append(TypeDescToStr(md.ArgumentTypes[i])).Append(", ");
                    }

                    if (md.ArgumentTypes.Count > 0) {
                        result.Append(TypeDescToStr(md.ArgumentTypes[md.ArgumentTypes.Count - 1]));
                    }

                    result.Append(')');
                    result.Append(TypeDescToStr(md.ReturnType));
                    return result.ToString();
                }
                default: return "DEBUG_ERROR_NOT_DESCRIPTOR: " + (value == null ? "null" : (value.GetType() + " -> " + value));
            }
            // throw new Exception("Cannot convert type " + value.GetType() + " to a string");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
