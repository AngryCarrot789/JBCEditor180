namespace BCEdit180.AttachedProperties {
    // Not being used; dont really have enough attached properties
    // for this to be useful tbh. but feel free to steal this if you want
    // xd, i got this from AngelSix btw

    ///// <summary>
    ///// A base class for simplifying the creation of attached properties 
    ///// <para>
    ///// This class is a bit confusing but it works so thats 
    ///// all that matters xdddd, and its quite simple to use too
    ///// </para>
    ///// </summary>
    ///// <typeparam name="Parent">The parent class (aka the class name of the </typeparam>
    ///// <typeparam name="Property">The Attacged Property type (bool, string, etc)</typeparam>
    //public class BaseAttachedProperty<Parent, Property> where Parent : BaseAttachedProperty<Parent, Property>, new()
    //{
    //    /// <summary>
    //    /// The "Singleton" Parent... somehow it gets set
    //    /// </summary>
    //    public static Parent Instance { get; private set; } = new Parent();
    //
    //    /// <summary>
    //    /// Attached property for this class
    //    /// </summary>
    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.RegisterAttached(
    //            "Value",
    //            typeof(Property),
    //            typeof(BaseAttachedProperty<Parent, Property>),
    //            new PropertyMetadata(
    //                new PropertyChangedCallback(OnValuePropertyChanged)));
    //
    //    public event Action<DependencyObject, DependencyPropertyChangedEventArgs> ValueChanged = (s, e) => { };
    //
    //    /// <summary>
    //    /// Callback event for when the property changes
    //    /// </summary>
    //    /// <param name="d">UI Element that has changed</param>
    //    /// <param name="e">Event args</param>
    //    private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        Instance.OnValueChanged(d, e);
    //        Instance.ValueChanged(d, e);
    //    }
    //
    //    /// <summary>
    //    /// Gets the attached property's value
    //    /// </summary>
    //    /// <param name="d">The element to get the property from</param>
    //    /// <returns></returns>
    //    public static Property GetValue(DependencyObject d)
    //    {
    //        return (Property)d.GetValue(ValueProperty);
    //    }
    //
    //    /// <summary>
    //    /// Sets the attached property's value
    //    /// </summary>
    //    /// <param name="d">The element to get the property from</param>
    //    /// <param name="value">The new value to set the property to</param>
    //    public static void SetValue(DependencyObject d, Property value)
    //    {
    //        d.SetValue(ValueProperty, value);
    //    }
    //
    //    /// <summary>
    //    /// Method that is called when any attached property is changed
    //    /// </summary>
    //    /// <param name="sender"></param>
    //    /// <param name="e"></param>
    //    public virtual void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) { }
    //}
}