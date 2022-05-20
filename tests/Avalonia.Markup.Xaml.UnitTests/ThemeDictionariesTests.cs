using Avalonia.Controls;
using Avalonia.Markup.Data;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Moq;
using Xunit;

namespace Avalonia.Markup.Xaml.UnitTests;

public class ThemeDictionariesTests : XamlTestBase
{
    [Fact]
    public void DynamicResource_Updated_When_Control_Theme_Changed()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <SolidColorBrush x:Key='DemoBackground'>Black</SolidColorBrush>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <SolidColorBrush x:Key='DemoBackground'>White</SolidColorBrush>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'/>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void DynamicResource_Updated_When_Control_Theme_Changed_No_Xaml()
    {
        var themeControl = new ThemeControl
        {
            Theme = ElementTheme.Light,
            Resources = new ResourceDictionary
            {
                ThemeDictionaries =
                {
                    [ElementTheme.Dark] = new ResourceDictionary { ["DemoBackground"] = Brushes.Black },
                    [ElementTheme.Light] = new ResourceDictionary { ["DemoBackground"] = Brushes.White }
                }
            },
            Child = new Border()
        };
        var border = (Border)themeControl.Child!;
        border[!Border.BackgroundProperty] = new DynamicResourceExtension("DemoBackground");
        
        DelayedBinding.ApplyBindings(border);
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }

    [Fact]
    public void Intermediate_DynamicResource_Updated_When_Control_Theme_Changed()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <Color x:Key='TestColor'>Black</Color>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <Color x:Key='TestColor'>White</Color>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
            <SolidColorBrush x:Key='DemoBackground' Color='{DynamicResource TestColor}' />
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'/>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }

    [Fact]
    public void Intermediate_StaticResource_Can_Be_Reached_From_ThemeDictionaries()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <Color x:Key='TestColor'>Black</Color>
                    <StaticResource x:Key='DemoBackground' ResourceKey='TestColor' />
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <Color x:Key='TestColor'>White</Color>
                    <StaticResource x:Key='DemoBackground' ResourceKey='TestColor' />
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'/>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }

    [Fact]
    public void StaticResource_Outside_Of_ThemeDictionaries_Should_Use_App_Theme()
    {
        using (AvaloniaLocator.EnterScope())
        {
            var applicationThemeHost = new Mock<IApplicationThemeHost>();
            applicationThemeHost.SetupGet(h => h.Theme).Returns(ElementTheme.Dark);
            AvaloniaLocator.CurrentMutable.Bind<IApplicationThemeHost>().ToConstant(applicationThemeHost.Object);

            var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <SolidColorBrush x:Key='DemoBackground'>Black</SolidColorBrush>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <SolidColorBrush x:Key='DemoBackground'>White</SolidColorBrush>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{StaticResource DemoBackground}'/>
</ThemeControl>");
            var border = (Border)themeControl.Child!;

            themeControl.Theme = ElementTheme.Light;
            Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
        }
    }
    
    [Fact]
    public void Inner_ThemeDictionaries_Works_Properly()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <Border Name='border' Background='{DynamicResource DemoBackground}'>
        <Border.Resources>
            <ResourceDictionary>
                <ResourceDictionary.ThemeDictionaries>
                    <ResourceDictionary x:Key='Dark'>
                        <SolidColorBrush x:Key='DemoBackground'>Black</SolidColorBrush>
                    </ResourceDictionary>
                    <ResourceDictionary x:Key='Light'>
                        <SolidColorBrush x:Key='DemoBackground'>White</SolidColorBrush>
                    </ResourceDictionary>
                </ResourceDictionary.ThemeDictionaries>
            </ResourceDictionary>
        </Border.Resources>
    </Border>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void Inner_Resource_Can_Reference_Parent_ThemeDictionaries()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <Color x:Key='TestColor'>Black</Color>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <Color x:Key='TestColor'>White</Color>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'>
        <Border.Resources>
            <ResourceDictionary>
                <SolidColorBrush x:Key='DemoBackground' Color='{DynamicResource TestColor}' />
            </ResourceDictionary>
        </Border.Resources>
    </Border>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void DynamicResource_Can_Access_Resources_Outside_Of_ThemeDictionaries()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <SolidColorBrush x:Key='DemoBackground' Color='{DynamicResource TestColor1}' />
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <SolidColorBrush x:Key='DemoBackground' Color='{DynamicResource TestColor2}' />
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
            <Color x:Key='TestColor1'>Black</Color>
            <Color x:Key='TestColor2'>White</Color>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}' />
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.White, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void Inner_Dictionary_Does_Not_Affect_Parent_Resources()
    {
        // It might be a nice feature, but neither Avalonia nor UWP supports it.
        // Better to expect this limitation with a unit test. 
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <Color x:Key='TestColor'>Red</Color>
            <SolidColorBrush x:Key='DemoBackground' Color='{DynamicResource TestColor}' />
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'>
        <Border.Resources>
            <ResourceDictionary>
                <ResourceDictionary.ThemeDictionaries>
                    <ResourceDictionary x:Key='Dark'>
                        <Color x:Key='TestColor'>Black</Color>
                    </ResourceDictionary>
                    <ResourceDictionary x:Key='Light'>
                        <Color x:Key='TestColor'>White</Color>
                    </ResourceDictionary>
                </ResourceDictionary.ThemeDictionaries>
            </ResourceDictionary>
        </Border.Resources>
    </Border>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        Assert.Equal(Colors.Red, ((ISolidColorBrush)border.Background)!.Color);

        themeControl.Theme = ElementTheme.Dark;

        Assert.Equal(Colors.Red, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void Custom_Theme_Can_Be_Defined_In_ThemeDictionaries()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <SolidColorBrush x:Key='DemoBackground'>Black</SolidColorBrush>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <SolidColorBrush x:Key='DemoBackground'>White</SolidColorBrush>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Custom'>
                    <SolidColorBrush x:Key='DemoBackground'>Pink</SolidColorBrush>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'/>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        themeControl.Theme = new ElementTheme("Custom");

        Assert.Equal(Colors.Pink, ((ISolidColorBrush)border.Background)!.Color);
    }
    
    [Fact]
    public void Custom_Theme_Fallbacks_To_Inherit_Theme()
    {
        var themeControl = (ThemeControl)AvaloniaRuntimeXamlLoader.Load(@"
<ThemeControl xmlns='https://github.com/avaloniaui'
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
              Theme='Light'>
    <ThemeControl.Resources>
        <ResourceDictionary>
            <ResourceDictionary.ThemeDictionaries>
                <ResourceDictionary x:Key='Dark'>
                    <SolidColorBrush x:Key='DemoBackground'>Black</SolidColorBrush>
                </ResourceDictionary>
                <ResourceDictionary x:Key='Light'>
                    <SolidColorBrush x:Key='DemoBackground'>White</SolidColorBrush>
                </ResourceDictionary>
            </ResourceDictionary.ThemeDictionaries>
        </ResourceDictionary>
    </ThemeControl.Resources>

    <Border Name='border' Background='{DynamicResource DemoBackground}'/>
</ThemeControl>");
        var border = (Border)themeControl.Child!;
        
        themeControl.Theme = new ElementTheme("Custom", ElementTheme.Dark);

        Assert.Equal(Colors.Black, ((ISolidColorBrush)border.Background)!.Color);
    }
}
