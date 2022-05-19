using System;

using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

#nullable enable

namespace Avalonia.Themes.Fluent
{
    public enum DensityStyle
    {
        Normal,
        Compact
    }

    /// <summary>
    /// Includes the fluent theme in an application.
    /// </summary>
    public partial class FluentTheme : Styles
    {
        private readonly Uri _baseUri;
        private IStyle _densityStyle = new Style();

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentTheme"/> class.
        /// </summary>
        /// <param name="baseUri">The base URL for the XAML context.</param>
        public FluentTheme(Uri baseUri)
        {
            _baseUri = baseUri;
            InitStyles(baseUri);

            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentTheme"/> class.
        /// </summary>
        /// <param name="serviceProvider">The XAML service provider.</param>
        public FluentTheme(IServiceProvider serviceProvider)
        {
            _baseUri = ((IUriContext)serviceProvider.GetService(typeof(IUriContext))).BaseUri;
            InitStyles(_baseUri);

            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<DensityStyle> DensityStyleProperty =
            AvaloniaProperty.Register<FluentTheme, DensityStyle>(nameof(DensityStyle));

        /// <summary>
        /// Gets or sets the density style of the fluent theme (normal, compact).
        /// </summary>
        public DensityStyle DensityStyle
        {
            get => GetValue(DensityStyleProperty);
            set => SetValue(DensityStyleProperty, value);
        }
        
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == DensityStyleProperty)
            {
                if (DensityStyle == DensityStyle.Compact)
                {
                    Add(_densityStyle);
                }
                else if (DensityStyle == DensityStyle.Normal)
                {
                    Remove(_densityStyle);
                }
            }
        }

        private void InitStyles(Uri baseUri)
        {
            _densityStyle = new StyleInclude(baseUri)
            {
                Source = new Uri("avares://Avalonia.Themes.Fluent/DensityStyles/Compact.xaml")
            };
        }
    }
}
