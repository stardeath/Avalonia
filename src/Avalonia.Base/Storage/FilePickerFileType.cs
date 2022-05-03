#nullable enable
using System.Collections.Generic;

namespace Avalonia.Storage
{
    public class FilePickerFileType
    {
        public FilePickerFileType(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IReadOnlyList<string>? Extensions { get; set; }
        // For web
        public IReadOnlyList<string>? MimeTypes { get; set; }
        // For Apple platforms
        public IReadOnlyList<string>? AppleUniformTypeIdentifiers { get; set; }
    }
}
