using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoffeeShop.ViewModels
{

    public interface IThemeSelectorService
    {
        ElementTheme GetTheme();
        void SetTheme(ElementTheme theme);
    }
    public class ThemeSelectorService : IThemeSelectorService
    {
        public ElementTheme GetTheme()
        {
            if (App.m_window?.Content is FrameworkElement frameworkElement)
            {
                return frameworkElement.ActualTheme;
            }
            return ElementTheme.Default;
        }

        public void SetTheme(ElementTheme theme)
        {
            if (App.m_window?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = theme;
            }
        }
    }
    public class MainViewModel : ObservableObject
    {

        private readonly IThemeSelectorService _themeSelectorService;

        public ICommand SetThemeCommand { get; }


        public MainViewModel(IThemeSelectorService themeSelectorService)
        {
            SetThemeCommand = new RelayCommand<string>((themeName) => UpdateTheme(themeName));
            _themeSelectorService = themeSelectorService;
            UpdateTheme("Light");
        }

        private void UpdateTheme(string? themeName)
        {
            if (Enum.TryParse(themeName, out ElementTheme theme) is true)
            {
                _themeSelectorService.SetTheme(theme);
            }
        }
        public ElementTheme GetTheme()
        {
            return _themeSelectorService.GetTheme();
        }
    }
}
