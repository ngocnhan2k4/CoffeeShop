using CoffeeShop.Views.Settings;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoffeeShop.Models;
using CoffeeShop.Service;

namespace CoffeeShop.ViewModels
{
 
    public class AppearanceViewModel : INotifyPropertyChanged
    {
        public Config EditedConfig { get; set; }

        public ObservableCollection<string> AvailableLanguages { get; } =
            new ObservableCollection<string> { "English", "Vietnamese" };

        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ILanguageSelectorService _languageService;
        public ICommand SetThemeCommand { get; }

        public AppearanceViewModel(IThemeSelectorService themeSelectorService, ILanguageSelectorService languageService)
        {
              SetThemeCommand = new RelayCommand<string>(value =>
              {
                  EditedConfig.Theme = value; 
              });

            _themeSelectorService = themeSelectorService;
            _languageService = languageService;
            EditedConfig = LoadSettings();

            // Subscribe to language changes from ComboBox
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(EditedConfig))
                {
                    UpdateLanguage(EditedConfig.Language);
                }
            };

            // Subscribe to language service changes
            _languageService.LanguageChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(EditedConfig));
            };
        }

        public void ResetSettings()
        {
            EditedConfig.Theme = "Light";
            EditedConfig.Language = "English";
            UpdateTheme(EditedConfig.Theme);
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
        private void UpdateLanguage(string language)
        {
            if (language != null && language != _languageService.GetCurrentLanguage())
            {
                _languageService.SetLanguage(language);
            }
        }

        public void SaveChanges()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "config.json");
            string json = JsonConvert.SerializeObject(EditedConfig, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
            
            // Update UI with saved settings
            UpdateTheme(EditedConfig.Theme);
            UpdateLanguage(EditedConfig.Language);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
