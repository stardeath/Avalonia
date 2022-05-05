#nullable enable
using System;

namespace Avalonia.Platform.Storage
{
    public class StorageItemProperties
    {
        public ulong? Size { get; set; }

        public DateTimeOffset? ItemDate { get; set; }

        public DateTimeOffset? DateModified { get; set; }
    }
}
