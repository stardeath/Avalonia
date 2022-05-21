using System;
using System.ComponentModel;

namespace Avalonia;

[TypeConverter(typeof(ElementThemeTypeConverter))]
public class ElementTheme
{
    public ElementTheme(object key)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
    }
        
    public ElementTheme(object key, ElementTheme inheritTheme)
    {
        Key = key ?? throw new ArgumentNullException(nameof(key));
        InheritTheme = inheritTheme;
    }

    public static ElementTheme Light { get; } = new(nameof(Light));
    public static ElementTheme Dark { get; } = new(nameof(Dark));

    public object Key { get; }

    public ElementTheme? InheritTheme { get; }
        
    public override int GetHashCode()
    {
        return Key.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is ElementTheme theme && Key.Equals(theme.Key);
    }

    public override string ToString()
    {
        return Key.ToString() ?? nameof(ElementTheme);
    }
}
