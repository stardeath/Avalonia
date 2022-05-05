using System.Collections.Generic;

namespace Avalonia.Platform.Storage
{
    /// <summary>
    /// Definition of file 
    /// </summary>
    public class FilePickerFileType
    {
        public FilePickerFileType(string name)
        {
            Name = name;
        }

        /// <summary>
        /// File type name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// List of 
        /// </summary>
        public IReadOnlyList<string>? Extensions { get; set; }

        /// <summary>
        /// List of extensions in MIME format.
        /// </summary>
        public IReadOnlyList<string>? MimeTypes { get; set; }

        /// <summary>
        /// List of extensions in Apple uniform format.
        /// </summary>
        /// <remarks>
        /// See https://developer.apple.com/documentation/uniformtypeidentifiers/system_declared_uniform_type_identifiers.
        /// </remarks>
        public IReadOnlyList<string>? AppleUniformTypeIdentifiers { get; set; }
    }
}
