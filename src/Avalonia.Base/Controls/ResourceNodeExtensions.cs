using System;
using Avalonia.LogicalTree;
using Avalonia.Reactive;

#nullable enable

namespace Avalonia.Controls
{
    public static class ResourceNodeExtensions
    {
        /// <summary>
        /// Finds the specified resource by searching up the logical tree and then global styles.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="key">The resource key.</param>
        /// <returns>The resource, or <see cref="AvaloniaProperty.UnsetValue"/> if not found.</returns>
        public static object? FindResource(this IResourceHost control, object key)
        {
            control = control ?? throw new ArgumentNullException(nameof(control));
            key = key ?? throw new ArgumentNullException(nameof(key));

            if (control.TryFindResource(key, out var value))
            {
                return value;
            }

            return AvaloniaProperty.UnsetValue;
        }

        /// <summary>
        /// Tries to the specified resource by searching up the logical tree and then global styles.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="key">The resource key.</param>
        /// <param name="value">On return, contains the resource if found, otherwise null.</param>
        /// <returns>True if the resource was found; otherwise false.</returns>
        public static bool TryFindResource(this IResourceHost control, object key, out object? value)
        {
            control = control ?? throw new ArgumentNullException(nameof(control));
            key = key ?? throw new ArgumentNullException(nameof(key));

            IResourceNode? current = control;

            while (current != null)
            {
                if (current.TryGetResource(key, out value))
                {
                    return true;
                }

                current = (current as IStyledElement)?.StylingParent as IResourceNode;
            }

            value = null;
            return false;
        }

        public static bool TryFindThemeResource(this IResourceHost control, ElementTheme theme, object key, out object? value)
        {
            control = control ?? throw new ArgumentNullException(nameof(control));
            theme = theme ?? throw new ArgumentNullException(nameof(theme));
            key = key ?? throw new ArgumentNullException(nameof(key));

            IResourceHost? current = control;

            while (current != null)
            {
                if (current is IResourceHost host)
                {
                    if (host.TryGetResource(theme, key, out value))
                    {
                        return true;
                    }
                }

                current = (current as IStyledElement)?.StylingParent as IResourceHost;
            }

            value = null;
            return false;
        }

        public static IObservable<object?> GetResourceObservable(
            this IResourceHost control,
            object key,
            Func<object?, object?>? converter = null)
        {
            control = control ?? throw new ArgumentNullException(nameof(control));
            key = key ?? throw new ArgumentNullException(nameof(key));

            return new ResourceObservable(control, key, converter);
        }

        public static IObservable<object?> GetResourceObservable(
            this IResourceProvider resourceProvider,
            object key,
            Func<object?, object?>? converter = null)
        {
            resourceProvider = resourceProvider ?? throw new ArgumentNullException(nameof(resourceProvider));
            key = key ?? throw new ArgumentNullException(nameof(key));

            return new FloatingResourceObservable(resourceProvider, key, converter);
        }

        private class ResourceObservable : LightweightObservableBase<object?>
        {
            private readonly IResourceHost _target;
            private readonly object _key;
            private readonly Func<object?, object?>? _converter;

            public ResourceObservable(IResourceHost target, object key, Func<object?, object?>? converter)
            {
                _target = target;
                _key = key;
                _converter = converter;
            }

            protected override void Initialize()
            {
                _target.ResourcesChanged += ResourcesChanged;
                if (_target is IThemeStyleable themeStyleable)
                {
                    themeStyleable.ThemeChanged += ThemeChanged;
                }
            }

            protected override void Deinitialize()
            {
                _target.ResourcesChanged -= ResourcesChanged;
                if (_target is IThemeStyleable themeStyleable)
                {
                    themeStyleable.ThemeChanged -= ThemeChanged;
                }
            }

            protected override void Subscribed(IObserver<object?> observer, bool first)
            {
                observer.OnNext(GetValue());
            }

            private void ResourcesChanged(object? sender, ResourcesChangedEventArgs e)
            {
                PublishNext(GetValue());
            }

            private void ThemeChanged(object? sender, EventArgs e)
            {
                PublishNext(GetValue());
            }

            private object? GetValue()
            {
                if (!(_target is IThemeStyleable themeStyleable)
                    || themeStyleable.Theme is null
                    || !themeStyleable.TryFindThemeResource(themeStyleable.Theme, _key, out var value))
                {
                    value = _target.FindResource(_key) ?? AvaloniaProperty.UnsetValue;
                }

                return _converter?.Invoke(value) ?? value;
            }
        }

        private class FloatingResourceObservable : LightweightObservableBase<object?>
        {
            private readonly IResourceProvider _target;
            private readonly object _key;
            private readonly Func<object?, object?>? _converter;
            private IResourceHost? _owner;

            public FloatingResourceObservable(IResourceProvider target, object key, Func<object?, object?>? converter)
            {
                _target = target;
                _key = key;
                _converter = converter;
            }

            protected override void Initialize()
            {
                _target.OwnerChanged += OwnerChanged;
                _owner = _target.Owner;
            }

            protected override void Deinitialize()
            {
                _target.OwnerChanged -= OwnerChanged;
                _owner = null;
            }

            protected override void Subscribed(IObserver<object?> observer, bool first)
            {
                if (_target.Owner is object)
                {
                    observer.OnNext(GetValue());
                }
            }

            private void PublishNext()
            {
                if (_target.Owner is object)
                {
                    PublishNext(GetValue());
                }
            }

            private void OwnerChanged(object? sender, EventArgs e)
            {
                if (_owner is object)
                {
                    _owner.ResourcesChanged -= ResourcesChanged;
                }
                if (_owner is IThemeStyleable themeStyleable)
                {
                    themeStyleable.ThemeChanged -= ThemeChanged;
                }

                _owner = _target.Owner;

                if (_owner is object)
                {
                    _owner.ResourcesChanged += ResourcesChanged;
                }
                if (_owner is IThemeStyleable themeStyleable2)
                {
                    themeStyleable2.ThemeChanged += ThemeChanged;
                }

                PublishNext();
            }

            private void ResourcesChanged(object? sender, ResourcesChangedEventArgs e)
            {
                PublishNext();
            }

            private void ThemeChanged(object? sender, EventArgs e)
            {
                PublishNext();
            }

            private object? GetValue()
            {
                if (!(_target.Owner is IThemeStyleable themeStyleable)
                    || themeStyleable.Theme is null
                    || !themeStyleable.TryFindThemeResource(themeStyleable.Theme, _key, out var value))
                {
                    value = _target.Owner?.FindResource(_key) ?? AvaloniaProperty.UnsetValue;
                }

                return _converter?.Invoke(value) ?? value;
            }
        }
    }
}
