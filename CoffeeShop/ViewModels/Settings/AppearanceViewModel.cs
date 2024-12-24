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

namespace CoffeeShop.ViewModels
{
 
    public class AppearanceViewModel : INotifyPropertyChanged
    {
        public Config EditedConfig { get; set; }
        public double PreviewFontSize { get; set; } = 16;
        public string SelectedLanguage { get; set; } = "English";

        public ObservableCollection<string> AvailableLanguages { get; } =
            new ObservableCollection<string> { "English", "Vietnamese" };

        private readonly IThemeSelectorService _themeSelectorService;

        public ICommand SetThemeCommand { get; }

        public AppearanceViewModel(IThemeSelectorService themeSelectorService)
        {
              SetThemeCommand = new RelayCommand<string>(value =>
              {
                  EditedConfig.Theme = value; 
                  UpdateTheme(value);
              });
            _themeSelectorService = themeSelectorService;
            EditedConfig = LoadSettings();
            UpdateTheme(EditedConfig.Theme);
        }

        public void ResetSettings()
        {
            EditedConfig.Theme = "Light";
            EditedConfig.Language = "English";
            EditedConfig.FontSize = 16;
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

        public void SaveChanges()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "config.json");
            string json = JsonConvert.SerializeObject(EditedConfig, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
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
    }
}
