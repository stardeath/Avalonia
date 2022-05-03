#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Native.Interop;
using Avalonia.Storage;
using Avalonia.Storage.FileIO;

namespace Avalonia.Native
{
    internal class MacOSStorageProvider : IStorageProvider
    {
        private readonly WindowBaseImpl _window;
        private readonly IAvnSystemDialogs _native;

        public MacOSStorageProvider(WindowBaseImpl window, IAvnSystemDialogs native)
        {
            _window = window;
            _native = native;
        }

        public bool CanOpen => true;

        public bool CanSave => true;

        public bool CanPickFolder => true;

        public Task<IStorageBookmarkFile?> OpenFileBookmarkAsync(string bookmark)
        {
            var file = new FileInfo(bookmark);
            return file.Exists
                ? Task.FromResult<IStorageBookmarkFile?>(new BclStorageFile(file))
                : Task.FromResult<IStorageBookmarkFile?>(null);
        }

        public Task<IStorageBookmarkFolder?> OpenFolderBookmarkAsync(string bookmark)
        {
            var folder = new DirectoryInfo(bookmark);
            return folder.Exists
                ? Task.FromResult<IStorageBookmarkFolder?>(new BclStorageFolder(folder))
                : Task.FromResult<IStorageBookmarkFolder?>(null);
        }

        public async Task<IReadOnlyList<IStorageFile>> OpenFilePickerAsync(FilePickerOpenOptions options)
        {
            using var events = new SystemDialogEvents();

            var suggestedDirectory = options.SuggestedStartLocation?.TryGetFullPath(out var suggestedDirectoryTmp) == true
                ? suggestedDirectoryTmp : string.Empty;

            _native.OpenFileDialog((IAvnWindow)_window.Native,
                                    events,
                                    options.AllowMultiple.AsComBool(),
                                    options.Title ?? string.Empty,
                                    suggestedDirectory,
                                    string.Empty,
                                    string.Join(";", options.FileTypeFilter?.SelectMany(f => f.Extensions ?? Array.Empty<string>()) ?? Array.Empty<string>()));

            var result = await events.Task.ConfigureAwait(false);

            return result?.Select(f => new BclStorageFile(new FileInfo(f))).ToArray() ?? Array.Empty<IStorageFile>();
        }

        public async Task<IStorageFile?> SaveFilePickerAsync(FilePickerSaveOptions options)
        {
            using var events = new SystemDialogEvents();

            var suggestedDirectory = options.SuggestedStartLocation?.TryGetFullPath(out var suggestedDirectoryTmp) == true
                ? suggestedDirectoryTmp : string.Empty;

            _native.SaveFileDialog((IAvnWindow)_window.Native,
                        events,
                        options.Title ?? string.Empty,
                        suggestedDirectory,
                        options.SuggestedFileName ?? string.Empty,
                        string.Join(";", options.FileTypeChoices?.SelectMany(f => f.Extensions ?? Array.Empty<string>()) ?? Array.Empty<string>()));

            var result = await events.Task.ConfigureAwait(false);
            return result.FirstOrDefault() is string file
                ? new BclStorageFile(new FileInfo(file))
                : null;
        }

        public async Task<IStorageFolder?> OpenFolderPickerAsync(FolderPickerOpenOptions options)
        {
            using var events = new SystemDialogEvents();

            var suggestedDirectory = options.SuggestedStartLocation?.TryGetFullPath(out var suggestedDirectoryTmp) == true
                ? suggestedDirectoryTmp : string.Empty;

            _native.SelectFolderDialog((IAvnWindow)_window.Native, events, options.Title ?? "", suggestedDirectory);

            var result = await events.Task.ConfigureAwait(false);
            return result.FirstOrDefault() is string folder
                ? new BclStorageFolder(new DirectoryInfo(folder))
                : null;
        }
    }

    internal unsafe class SystemDialogEvents : NativeCallbackBase, IAvnSystemDialogEvents
    {
        private readonly TaskCompletionSource<string[]> _tcs;

        public SystemDialogEvents()
        {
            _tcs = new TaskCompletionSource<string[]>();
        }

        public Task<string[]> Task => _tcs.Task;

        public void OnCompleted(int numResults, void* trFirstResultRef)
        {
            string[] results = new string[numResults];

            unsafe
            {
                var ptr = (IntPtr*)trFirstResultRef;

                for (int i = 0; i < numResults; i++)
                {
                    results[i] = Marshal.PtrToStringAnsi(*ptr) ?? string.Empty;

                    ptr++;
                }
            }

            _tcs.SetResult(results);
        }
    }
}
