using System;
using System.ComponentModel;
using System.Globalization;

namespace Avalonia;

public class ElementThemeTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            nameof(ElementTheme.Light) => ElementTheme.Light,
            nameof(ElementTheme.Dark) => ElementTheme.Dark,
            _ => new ElementTheme(value)
        };
    }
}
