using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BCEdit180.Utils {
    public class AnimationHelpers {
        //Made by ADeltaX i think

        /// <summary>
        /// Use this method to make an animation for a control in Y axis
        /// </summary>
        /// <param name="cntrl">The targhetting Control</param>
        /// <param name="YPos">Here the position to add</param>
        /// <param name="TimeSecond">The duration of the animation</param>
        /// <param name="TimeMillisecond">The delay of the animation</param>
        public static void MoveToTargetY(Control cntrl, double From, double To, double TimeSecond, double TimeMillisecond = 0) {
            cntrl.Margin = new Thickness(cntrl.Margin.Left, cntrl.Margin.Top - To, cntrl.Margin.Right, cntrl.Margin.Bottom + To);
            //QuadraticEase EP = new QuadraticEase();
            //EP.EasingMode = EasingMode.EaseOut;

            DoubleAnimation DirY = new DoubleAnimation {
                Duration = new Duration(TimeSpan.FromSeconds(TimeSecond)),
                From = From,
                To = To,
                BeginTime = TimeSpan.FromMilliseconds(TimeMillisecond),
                //EasingFunction = EP,
                AutoReverse = false
            };
            SetAnimationRatios(DirY);
            cntrl.RenderTransform = new TranslateTransform();
            cntrl.RenderTransform.BeginAnimation(TranslateTransform.YProperty, DirY);
        }

        /// <summary>
        /// Use this method to make an animation for a control in X axis
        /// </summary>
        /// <param name="cntrl">The targhetting Control</param>
        /// <param name="XPos">Here the position to add</param>
        /// <param name="TimeSecond">The duration of the animation</param>
        /// <param name="TimeMillisecond">The delay of the animation</param>
        public static void MoveToTargetX(Control control, double From, double To, double TimeSecond, double TimeMillisecond = 0) {
            control.Margin = new Thickness(control.Margin.Left - To, control.Margin.Top, control.Margin.Right + To, control.Margin.Bottom);
            //QuinticEase EP = new QuinticEase();
            //EP.EasingMode = EasingMode.EaseOut;

            DoubleAnimation DirX = new DoubleAnimation {
                Duration = new Duration(TimeSpan.FromSeconds(TimeSecond)),
                From = From,
                To = To,
                BeginTime = TimeSpan.FromMilliseconds(TimeMillisecond),
                //EasingFunction = EP,
                AutoReverse = false
            };
            SetAnimationRatios(DirX);
            control.RenderTransform = new TranslateTransform();
            control.RenderTransform.BeginAnimation(TranslateTransform.XProperty, DirX);
        }

        public static void OpacityControl(Control control, double From, double To, double TimeSecond, double TimeMillisecond = 0) {
            //QuinticEase EP = new QuinticEase();
            //EP.EasingMode = EasingMode.EaseOut;

            DoubleAnimation Dir = new DoubleAnimation {
                Duration = new Duration(TimeSpan.FromSeconds(TimeSecond)),
                From = From,
                To = To,
                BeginTime = TimeSpan.FromMilliseconds(TimeMillisecond),
                //EasingFunction = EP,
                AutoReverse = false
            };
            SetAnimationRatios(Dir);
            control.BeginAnimation(UIElement.OpacityProperty, Dir);
        }

        public static void SetAnimationRatios(Timeline timeline) {
            timeline.AccelerationRatio = 0;
            timeline.DecelerationRatio = 1;
        }
    }

    [ContentProperty("Actions")]
    public class ConditionalEventTrigger : FrameworkContentElement {
        public RoutedEvent RoutedEvent { get; set; }
        public List<TriggerAction> Actions { get; set; }

        // Condition
        public bool Condition { get { return (bool) this.GetValue(ConditionProperty); } set { this.SetValue(ConditionProperty, value); } }
        public static readonly DependencyProperty ConditionProperty = DependencyProperty.Register("Condition", typeof(bool), typeof(ConditionalEventTrigger));

        // "Triggers" attached property
        public static ConditionalEventTriggerCollection GetTriggers(DependencyObject obj) { return (ConditionalEventTriggerCollection) obj.GetValue(TriggersProperty); }
        public static void SetTriggers(DependencyObject obj, ConditionalEventTriggerCollection value) { obj.SetValue(TriggersProperty, value); }

        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("Triggers", typeof(ConditionalEventTriggerCollection), typeof(ConditionalEventTrigger), new PropertyMetadata {
            PropertyChangedCallback = (obj, e) => {
                // When "Triggers" is set, register handlers for each trigger in the list 
                var element = (FrameworkElement) obj;
                var triggers = (List<ConditionalEventTrigger>) e.NewValue;
                foreach (var trigger in triggers)
                    element.AddHandler(trigger.RoutedEvent, new RoutedEventHandler((obj2, e2) =>
                        trigger.OnRoutedEvent(element)));
            }
        });

        public ConditionalEventTrigger() {
            this.Actions = new List<TriggerAction>();
        }

        // When an event fires, check the condition and if it is true fire the actions 
        void OnRoutedEvent(FrameworkElement element) {
            this.DataContext = element.DataContext; // Allow data binding to access element properties
            if (this.Condition) {
                // Construct an EventTrigger containing the actions, then trigger it 
                var dummyTrigger = new EventTrigger {RoutedEvent = _triggerActionsEvent};
                foreach (var action in this.Actions)
                    dummyTrigger.Actions.Add(action);

                element.Triggers.Add(dummyTrigger);
                try {
                    element.RaiseEvent(new RoutedEventArgs(_triggerActionsEvent));
                }
                finally {
                    element.Triggers.Remove(dummyTrigger);
                }
            }
        }

        static RoutedEvent _triggerActionsEvent = EventManager.RegisterRoutedEvent("", RoutingStrategy.Direct, typeof(EventHandler), typeof(ConditionalEventTrigger));
    }

    // Create collection type visible to XAML - since it is attached we cannot construct it in code 
    public class ConditionalEventTriggerCollection : List<ConditionalEventTrigger> {
    }
}