using System;

using Avalonia.Controls;

#nullable enable

namespace Avalonia
{
    public interface IThemeStyleable : IResourceHost
    {
        /// <summary>
        /// Element theme. Inherits value from the parent.
        /// </summary>
        ElementTheme Theme { get; }

        event EventHandler? ThemeChanged;
    }

    public interface IApplicationThemeHost : IThemeStyleable
    {

    }
}
