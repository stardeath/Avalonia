using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ControlCatalog.Pages
{
    public class ThemePage : UserControl
    {
        public static ElementTheme Pink { get; } = new("Pink", ElementTheme.Light);
        
        public ThemePage()
        {
            AvaloniaXamlLoader.Load(this);

            var selector = this.FindControl<ComboBox>("Selector")!;
            var themeControl = this.FindControl<ThemeControl>("ThemeControl")!;

            selector.Items = new[]
            {
                new ElementTheme("Default"),
                ElementTheme.Dark,
                ElementTheme.Light,
                Pink
            };
            selector.SelectedIndex = 0;

            selector.SelectionChanged += (_, _) =>
            {
                var theme = (ElementTheme)selector.SelectedItem!;
                if ((string)theme.Key == "Default")
                {
                    themeControl.ClearValue(ThemeControl.ThemeProperty);
                }
                else
                {
                    themeControl.Theme = theme;
                }
            };
        }
    }
}
