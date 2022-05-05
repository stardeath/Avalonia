#nullable enable

namespace Avalonia.Platform.Storage
{
    public class FolderPickerOpenOptions
    {
        public IStorageFolder? SuggestedStartLocation { get; set; }
        public string? Title { get; set; }
    }
}
