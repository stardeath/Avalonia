using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;

namespace Avalonia.Web.Blazor.Storage
{
    internal class BlazorFilePicker : IStorageProvider
    {
        public bool CanOpen => throw new NotImplementedException();

        public bool CanSave => throw new NotImplementedException();

        public bool CanExport => throw new NotImplementedException();

        public bool CanPickFolder => throw new NotImplementedException();

        public Task Export(FilePickerSaveOptions options, Func<Stream, FilePickerFileType, Task> writer)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageFolder?> OpenFolderPickerAsync(FolderPickerOpenOptions options)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
        {
            throw new NotImplementedException();
        }
    }
}
