using System;
using System.Windows;
using System.Windows.Controls;
using BCEdit180.Core.Utils;

namespace BCEdit180.Controls {
    public static class TreeItemExtension {
        public static readonly DependencyProperty IsInitiallyExpandableProperty = DependencyProperty.RegisterAttached("IsInitiallyExpandable", typeof(bool), typeof(TreeItemExtension), new PropertyMetadata(BoolBox.False, PropertyChangedCallback));

        public static void SetIsInitiallyExpandable(DependencyObject element, bool value) => element.SetValue(IsInitiallyExpandableProperty, value.Box());

        public static bool GetIsInitiallyExpandable(DependencyObject element) => (bool) element.GetValue(IsInitiallyExpandableProperty);

        private static readonly RoutedEventHandler ExpandedHandler = OnItemExpanded;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (!(d is TreeViewItem item))
                throw new ArgumentException("Object must be tree view item");
            if ((bool) e.NewValue)
                item.Expanded += ExpandedHandler;
        }

        private static void OnItemExpanded(object sender, RoutedEventArgs e) {
            TreeViewItem item = (TreeViewItem) sender;
            item.Expanded -= ExpandedHandler;
            item.ClearValue(IsInitiallyExpandableProperty);
        }
    }
}