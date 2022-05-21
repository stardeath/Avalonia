namespace Avalonia.Controls
{
    /// <summary>
    /// Decorator control that isolates controls subtree with locally defined <see cref="Theme"/> property.
    /// </summary>
    public class ThemeControl : Decorator
    {
        /// <summary>
        /// Defines the <see cref="Theme"/> property.
        /// </summary>
        public static readonly StyledProperty<ElementTheme> ThemeProperty =
            AvaloniaProperty.Register<ThemeControl, ElementTheme>(
                nameof(Theme),
                inherits: true,
                defaultValue: ElementTheme.Light);

        /// <summary>
        /// Gets or sets the UI theme that is used by the control (and its child elements) for resource determination.
        /// The UI theme you specify with RequestedTheme can override the app-level RequestedTheme.
        /// </summary>
        /// <remarks>
        /// To reset local value and inherit parent theme, call <see cref="ThemeControl.ClearValue(AvaloniaProperty)" /> with <see cref="ThemeProperty"/> as an argument.
        /// </remarks>
        public ElementTheme Theme
        {
            get => GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
    }
}
