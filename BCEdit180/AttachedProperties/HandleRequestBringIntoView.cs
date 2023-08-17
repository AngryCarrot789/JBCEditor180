using System;
using System.Windows;
using System.Windows.Controls;
using BCEdit180.Core.Utils;

namespace BCEdit180.AttachedProperties {
    public static class HandleRequestBringIntoView {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(HandleRequestBringIntoView),
                new PropertyMetadata(BoolBox.False, PropertyChangedCallback));

        public static void SetIsEnabled(DependencyObject element, bool value) => element.SetValue(IsEnabledProperty, value);

        public static bool GetIsEnabled(DependencyObject element) => (bool)element.GetValue(IsEnabledProperty);

        private static RequestBringIntoViewEventHandler EventHandler;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is FrameworkElement element) {
                RequestBringIntoViewEventHandler handler = EventHandler ?? (EventHandler = GridOnRequestBringIntoView);
                element.RequestBringIntoView -= handler;
                if ((bool) e.NewValue) {
                    element.RequestBringIntoView += handler;
                }
            }
        }

        private static void GridOnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e) {
            // Prevent a parent ItemsControl from scrolling when a child is selected while partially off screen
            e.Handled = true;
        }
    }
}