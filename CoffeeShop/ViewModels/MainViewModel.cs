using CoffeeShop.Models;
using CoffeeShop.Service;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (App.m_window?.Content is FrameworkElement frameworkElement && App.m_window.AppWindow != null)
            {
                return frameworkElement.ActualTheme;
            }
            return ElementTheme.Default;
        }

        public void SetTheme(ElementTheme theme)
        {
            if ( App.m_window?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = theme;
            }
        }
    }

    public class MainViewModel
    {
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ILanguageSelectorService _languageService;
        
        private Config _config;

        public ICommand SetThemeCommand { get; }


        public MainViewModel(IThemeSelectorService themeSelectorService, ILanguageSelectorService languageService)
        {
            _themeSelectorService = themeSelectorService;
            _languageService = languageService;
            _config = LoadSettings();
        }
        public ElementTheme GetTheme()
        {
            return _themeSelectorService.GetTheme();
        }
        public string GetLanguage()
        {
            return _languageService.GetCurrentLanguage();
        }
        public static Config LoadSettings()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "config.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                Config config = JsonConvert.DeserializeObject<Config>(json);
                return config;
            }
            return new Config();
        }

        public void ApplyTheme()
        {
            if (Enum.TryParse(_config.Theme, out ElementTheme theme))
            {
                _themeSelectorService.SetTheme(theme);
            }
        }

        public void ApplyLanguage()
        {
            _languageService.SetLanguage(_config.Language);
        }
    }
}
