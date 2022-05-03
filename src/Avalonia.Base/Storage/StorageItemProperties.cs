#nullable enable
using System;

namespace Avalonia.Storage
{
    public class StorageItemProperties
    {
        public ulong? Size { get; set; }

        public DateTimeOffset? ItemDate { get; set; }

        public DateTimeOffset? DateModified { get; set; }
    }
}
