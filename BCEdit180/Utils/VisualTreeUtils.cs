using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace BCEdit180.Utils {
    public static class VisualTreeUtils {
        /// <summary>
        /// Returns the control which has the given inherited property defined
        /// </summary>
        /// <param name="property"></param>
        /// <param name="startObject"></param>
        /// <returns></returns>
        public static DependencyObject FindNearestInheritedPropertyDefinition(DependencyProperty property, DependencyObject startObject) {
            DependencyObject obj = startObject;
            while (obj != null && obj.ReadLocalValue(property) == DependencyProperty.UnsetValue) {
                obj = GetParent(obj);
            }

            return obj;
        }

        public static DependencyObject GetParent(DependencyObject source) {
            if (source is Visual || source is Visual3D) {
                return VisualTreeHelper.GetParent(source);
            }
            else if (source is FrameworkContentElement fce) {
                return fce.Parent;
            }
            else {
                return null;
            }
        }

        public static T FindParent<T>(DependencyObject obj, bool includeSelf = true) where T : DependencyObject {
            if (obj == null || includeSelf && obj is T) {
                return (T) obj;
            }

            do {
                obj = GetParent(obj);
            } while (obj != null && !(obj is T));

            return (T) obj;
        }

        public static T FindVisualChild<T>(DependencyObject obj, bool useSelf = true) where T : DependencyObject {
            return useSelf && obj is T ? (T) obj : FindChildRecursive<T>(obj, null);
        }

        public static T FindVisualChildByName<T>(DependencyObject obj, string name, bool useSelf = true) where T : FrameworkElement {
            return useSelf && obj is T && ((T) obj).Name == name ? (T) obj : FindChildRecursive<T>(obj, x => x.Name == name);
        }

        private static T FindChildRecursive<T>(DependencyObject obj, Predicate<T> filter) where T : DependencyObject {
            int count, i;
            if (obj is ContentControl) {
                DependencyObject child = ((ContentControl) obj).Content as DependencyObject;
                if (child is T && (filter == null || filter((T) child))) {
                    return (T) child;
                }
                else {
                    return child != null ? FindChildRecursive(child, filter) : null;
                }
            }
            else if ((obj is Visual || obj is Visual3D) && (count = VisualTreeHelper.GetChildrenCount(obj)) > 0) {
                for (i = 0; i < count; i++) {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child is T && (filter == null || filter((T) child))) {
                        return (T) child;
                    }
                }

                for (i = 0; i < count; i++) {
                    T child = FindChildRecursive(VisualTreeHelper.GetChild(obj, i), filter);
                    if (child != null && (filter == null || filter(child))) {
                        return child;
                    }
                }
            }

            return null;
        }

        public static object GetDataContext(DependencyObject value) {
            if (value is FrameworkElement) {
                return ((FrameworkElement) value).DataContext;
            }
            else if (value is FrameworkContentElement) {
                return ((FrameworkContentElement) value).DataContext;
            }
            else {
                return null;
            }
        }

        public static bool GetDataContextHelper(DependencyObject src, out object context) => (context = GetDataContext(src)) != null;
    }
}