#nullable enable
using System.Collections.Generic;

namespace Avalonia.Storage
{
    public class FilePickerOpenOptions
    {
        public string? Title { get; set; }
        public IStorageFolder? SuggestedStartLocation    { get; set; }
        public bool AllowMultiple { get; set; }
        public IReadOnlyList<FilePickerFileType>? FileTypeFilter { get; set; }
    }
}
