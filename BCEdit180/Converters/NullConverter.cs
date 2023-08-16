using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BCEdit180.Core.Utils;

namespace BCEdit180.Converters {
    public class NullConverter : IValueConverter {
        public object NullValue { get; set; }
        public object NonNullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value == null ? this.NullValue : this.NonNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class NullToVisibilityConverter : NullConverter {
        internal static readonly object _VisibleBox = Visibility.Visible;
        internal static readonly object _HiddenBox = Visibility.Hidden;
        internal static readonly object _CollapsedBox = Visibility.Collapsed;

        public static NullToVisibilityConverter ToVisibleOrCollapsed { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Visible, NonNullValue = Visibility.Collapsed};
        public static NullToVisibilityConverter ToVisibleOrHidden { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Visible, NonNullValue = Visibility.Hidden};
        public static NullToVisibilityConverter ToHiddenOrVisible { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Hidden, NonNullValue = Visibility.Visible};
        public static NullToVisibilityConverter ToHiddenOrCollapsed { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Hidden, NonNullValue = Visibility.Collapsed};
        public static NullToVisibilityConverter ToCollapsedOrVisible { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Collapsed, NonNullValue = Visibility.Visible};
        public static NullToVisibilityConverter ToCollapsedOrHidden { get; } = new NullToVisibilityConverter() { NullValue = Visibility.Collapsed, NonNullValue = Visibility.Hidden};

        public new Visibility NullValue {
            get => (Visibility) base.NullValue;
            set => base.NullValue = Box(value);
        }

        public new Visibility NonNullValue {
            get => (Visibility) base.NonNullValue;
            set => base.NonNullValue = Box(value);
        }

        public static object Box(Visibility visibility) {
            switch (visibility) {
                case Visibility.Visible: return _VisibleBox;
                case Visibility.Hidden: return _HiddenBox;
                case Visibility.Collapsed: return _CollapsedBox;
                default: return visibility; // bit flags???
            }
        }

        public NullToVisibilityConverter() {
            base.NullValue = _HiddenBox;
            base.NonNullValue = _VisibleBox;
        }
    }

    public class NullToBoolConverter : NullConverter {
        public static NullToBoolConverter ToTrue { get; } = new NullToBoolConverter() {NullValue = true, NonNullValue = false};
        public static NullToBoolConverter ToFalse { get; } = new NullToBoolConverter() {NullValue = false, NonNullValue = true};

        public new bool NullValue {
            get => (bool) base.NullValue;
            set => base.NullValue = value.Box();
        }

        public new bool NonNullValue {
            get => (bool) base.NonNullValue;
            set => base.NonNullValue = value.Box();
        }

        public NullToBoolConverter() {
            base.NullValue = BoolBox.False;
            base.NonNullValue = BoolBox.True;
        }
    }
}