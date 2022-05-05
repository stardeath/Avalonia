#nullable enable
using System.Collections.Generic;

namespace Avalonia.Platform.Storage
{
    public class FilePickerSaveOptions
    {
        public string? Title { get; set; }
        public string? SuggestedFileName { get; set; }
        public IStorageFolder? SuggestedStartLocation { get; set; }
        public IReadOnlyList<FilePickerFileType>? FileTypeChoices { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display a warning if the user specifies the name of a file that already exists.
        /// </summary>
        public bool? ShowOverwritePrompt { get; set; }
    }
}
