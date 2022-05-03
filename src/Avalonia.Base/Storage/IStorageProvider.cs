#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avalonia.Storage
{
    public interface IStorageProvider
    {
        bool CanOpen { get; }
        Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options);

        bool CanSave { get; }
        Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options);

        bool CanPickFolder { get; }
        Task<IStorageFolder?> OpenFolderPickerAsync(FolderPickerOpenOptions options);
        
        Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark);

        Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark);
    }
}
