using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using BCEdit180.Core;
using BCEdit180.Core.Utils;
using BCEdit180.Resources;

namespace BCEdit180.AdvancedContextService {
    public class AdvancedMenuItem : MenuItem {
        private object currentItem;

        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register("IconType", typeof(IconType), typeof(AdvancedMenuItem), new PropertyMetadata(IconType.None, PropertyChangedCallback));
        public static readonly DependencyProperty CommandParameterTargetTypeProperty = DependencyProperty.Register("CommandParameterTargetType", typeof(Type), typeof(AdvancedMenuItem), new PropertyMetadata(null));
        public static readonly DependencyProperty ForceCommandOnMissingTargetTypeProperty = DependencyProperty.Register("ForceCommandOnMissingTargetType", typeof(bool), typeof(AdvancedMenuItem), new PropertyMetadata(BoolBox.False));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is AdvancedMenuItem item) {
                if (IconTypeToImageSourceConverter.IconTypeToImageSource((IconType) e.NewValue) is ImageSource x) {
                    Image image = new Image {
                        Source = x, Stretch = Stretch.Uniform,
                        SnapsToDevicePixels = true,
                        UseLayoutRounding = true
                    };

                    RenderOptions.SetEdgeMode(image, EdgeMode.Aliased);
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.LowQuality);
                    item.UseLayoutRounding = true;
                    item.Icon = image;
                }
            }
        }

        public IconType IconType {
            get => (IconType) this.GetValue(IconTypeProperty);
            set => this.SetValue(IconTypeProperty, value);
        }

        /// <summary>
        /// Used to search for a specific type of object to pass as a command parameter
        /// </summary>
        public Type CommandParameterTargetType {
            get => (Type) this.GetValue(CommandParameterTargetTypeProperty);
            set => this.SetValue(CommandParameterTargetTypeProperty, value);
        }

        public bool ForceCommandOnMissingTargetType {
            get => (bool) this.GetValue(ForceCommandOnMissingTargetTypeProperty);
            set => this.SetValue(ForceCommandOnMissingTargetTypeProperty, value.Box());
        }

        static AdvancedMenuItem() {
        }

        public AdvancedMenuItem() {
        }

        private static readonly MethodInfo FocusOrSelectMethodInfo = typeof(MenuItem).GetMethod("FocusOrSelect", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

        // protected override void OnClick() {
        //     // base.OnClick();
        //     if (this.IsCheckable)
        //         this.IsChecked = !this.IsChecked;
        //     if (!this.IsKeyboardFocusWithin)
        //         FocusOrSelectMethodInfo.Invoke(this, new object[0]);
        //     // this.RaiseEvent(new RoutedEventArgs(MenuItem.PreviewClickEvent, (object) this));
        //     this.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(this.InvokeClickAfterRender), null);
        // }
        // private object InvokeClickAfterRender(object arg) {
        //     ICommand command = this.Command;
        //     if (command != null) {
        //         object param = null;
        //         if (this.CommandParameterTargetType is Type type) {
        //             IInputElement element = Keyboard.FocusedElement;
        //         }
        //         if (param == null) {
        //             param = this.CommandParameter;
        //         }
        //         if (command.CanExecute(param)) {
        //             command.Execute(param);
        //         }
        //     }
        //     return null;
        // }

        protected override bool IsItemItsOwnContainerOverride(object item) {
            if (item is MenuItem || item is Separator)
                return true;
            this.currentItem = item;
            return false;
        }

        protected override DependencyObject GetContainerForItemOverride() {
            object item = this.currentItem;
            this.currentItem = null;
            if (this.UsesItemContainerTemplate) {
                DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(item, this);
                if (dataTemplate != null) {
                    object obj = dataTemplate.LoadContent();
                    if (obj is MenuItem || obj is Separator) {
                        return (DependencyObject) obj;
                    }

                    throw new InvalidOperationException("Invalid data template object: " + obj);
                }
            }

            return AdvancedContextMenu.CreateChildMenuItem(item);
        }
    }
}