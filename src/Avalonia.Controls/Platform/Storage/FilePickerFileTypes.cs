#nullable enable
namespace Avalonia.Platform.Storage
{
    /// <summary>
    /// Dictionary of known file types.
    /// </summary>
    public class FilePickerFileTypes
    {
        public static FilePickerFileType All { get; } = new FilePickerFileType("All")
        {
            Extensions = new[] { "*" },
            MimeTypes = new[] { "*/*" }
        };

        public static FilePickerFileType TextPlain { get; } = new FilePickerFileType("Plain Text")
        {
            Extensions = new[] { "txt" },
            AppleUniformTypeIdentifiers = new[] { "public.plain-text" },
            MimeTypes = new[] { "text/plain" }
        };

        public static FilePickerFileType ImageAll { get; } = new FilePickerFileType("All Images")
        {
            Extensions = new[] { "png", "jpg", "jpeg", "gif", "bmp" },
            AppleUniformTypeIdentifiers = new[] { "public.image" },
            MimeTypes = new[] { "image/*" }
        };

        public static FilePickerFileType ImageJpg { get; } = new FilePickerFileType("JPEG image")
        {
            Extensions = new[] { "jpg", "jpeg" },
            AppleUniformTypeIdentifiers = new[] { "public.jpeg" },
            MimeTypes = new[] { "image/jpeg" }
        };

        public static FilePickerFileType ImagePng { get; } = new FilePickerFileType("PNG image")
        {
            Extensions = new[] { "png" },
            AppleUniformTypeIdentifiers = new[] { "public.png" },
            MimeTypes = new[] { "image/png" }
        };

        public static FilePickerFileType Pdf { get; } = new FilePickerFileType("PDF document")
        {
            Extensions = new[] { "pdf" },
            AppleUniformTypeIdentifiers = new[] { "com.adobe.pdf" },
            MimeTypes = new[] { "application/pdf" }
        };
    }
}
