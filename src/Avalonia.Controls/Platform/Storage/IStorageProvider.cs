#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;

using Avalonia.Controls;

namespace Avalonia.Platform.Storage
{
    public interface IStorageProvider
    {
        bool CanOpen { get; }
        Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(TopLevel topLevel, FilePickerOpenOptions options);

        bool CanSave { get; }
        Task<IStorageFile?> SaveFilePickerAsync(TopLevel topLevel, FilePickerSaveOptions options);

        bool CanPickFolder { get; }
        Task<IStorageFolder?> OpenFolderPickerAsync(TopLevel topLevel, FolderPickerOpenOptions options);
        
        Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark);

        Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark);
    }
}
