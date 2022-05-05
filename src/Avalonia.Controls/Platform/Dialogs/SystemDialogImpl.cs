using System;
using System.Linq;
using System.Threading.Tasks;

using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;

#nullable enable

namespace Avalonia.Controls.Platform
{
    /// <summary>
    /// Defines a platform-specific system dialog implementation.
    /// </summary>
    [Obsolete]
    internal class SystemDialogImpl : ISystemDialogImpl
    {
        public async Task<string[]?> ShowFileDialogAsync(FileDialog dialog, Window parent)
        {
            var types = dialog.Filters.Select(f => new FilePickerFileType(f.Name!) { Extensions = f.Extensions }).ToArray();
            if (dialog is OpenFileDialog openDialog)
            {
                Application.Current.Clipboard
                var filePicker = parent.StorageProvider;
                if (!filePicker.CanOpen)
                {
                    return null;
                }

                var options = new FilePickerOpenOptions
                {
                    AllowMultiple = openDialog.AllowMultiple,
                    FileTypeFilter = types,
                    Title = dialog.Title,
                    SuggestedStartLocation = openDialog.InitialDirectory is { } directory
                        ? new BclStorageFolder(new System.IO.DirectoryInfo(directory))
                        : null
                };

                var files = await filePicker.OpenFilePickerAsync(parent, options);
                return files
                    .Select(file => file.TryGetFullPath(out var fullPath)
                        ? fullPath
                        : file.Name)
                    .ToArray();
            }
            else if (dialog is SaveFileDialog saveDialog)
            {
                var filePicker = parent.StorageProvider;
                if (!filePicker.CanSave)
                {
                    return null;
                }

                var options = new FilePickerSaveOptions
                {
                    SuggestedFileName = saveDialog.InitialFileName,
                    FileTypeChoices = types,
                    Title = dialog.Title,
                    SuggestedStartLocation = saveDialog.InitialDirectory is { } directory
                        ? new Storage.FileIO.BclStorageFolder(new System.IO.DirectoryInfo(directory))
                        : null
                };

                var file = await filePicker.SaveFilePickerAsync(options);
                if (file is null)
                {
                    return null;
                }

                var filePath = file.TryGetFullPath(out var fullPath)
                    ? fullPath
                    : file.Name;
                return new[] { filePath };
            }
            return null;
        }

        public async Task<string?> ShowFolderDialogAsync(OpenFolderDialog dialog, Window parent)
        {
            var filePicker = parent.StorageProvider;
            if (!filePicker.CanPickFolder)
            {
                return null;
            }

            var options = new FolderPickerOpenOptions
            {
                Title = dialog.Title,
                SuggestedStartLocation = dialog.InitialDirectory is { } directory
                    ? new Storage.FileIO.BclStorageFolder(new System.IO.DirectoryInfo(directory))
                    : null
            };

            var folder = await filePicker.OpenFolderPickerAsync(options);
            return folder is not null && folder.TryGetFullPath(out var fullPath) ? fullPath : null;
        }
    }
}
