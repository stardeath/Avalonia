namespace Avalonia.Controls
{
    public class ThemeControl : Decorator
    {
        public static readonly StyledProperty<ElementTheme> ThemeProperty =
            AvaloniaProperty.Register<ThemeControl, ElementTheme>(
                nameof(Theme),
                inherits: true,
                defaultValue: ElementTheme.Light);

        public ElementTheme Theme
        {
            get => GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
    }
}
