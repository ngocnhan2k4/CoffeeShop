using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.ViewModels;
using CoffeeShop.Models;
using CoffeeShop.Service;
using System.IO;
using Newtonsoft.Json;
using System;
using Microsoft.UI.Xaml;

namespace CoffeeShopTests.ViewModels
{
    [TestClass]
    public class AppearanceViewModelTests
    {
        private AppearanceViewModel _viewModel;
        private MockThemeSelectorService _mockThemeSelectorService;
        private MockLanguageSelectorService _mockLanguageSelectorService;
        private string _testFilePath;

        [TestInitialize]
        public void Setup()
        {
            _mockThemeSelectorService = new MockThemeSelectorService();
            _mockLanguageSelectorService = new MockLanguageSelectorService();
            _viewModel = new AppearanceViewModel(_mockThemeSelectorService, _mockLanguageSelectorService);

            _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public void SetThemeCommand_UpdatesThemeCorrectly()
        {
            // Act
            _viewModel.SetThemeCommand.Execute("Dark");

            // Assert
            Assert.AreEqual("Dark", _viewModel.EditedConfig.Theme);
        }

        [TestMethod]
        public void ResetSettings_ResetsToDefaultValues()
        {
            // Arrange
            _viewModel.EditedConfig.Theme = "Dark";
            _viewModel.EditedConfig.Language = "Vietnamese";

            // Act
            _viewModel.ResetSettings();

            // Assert
            Assert.AreEqual("Light", _viewModel.EditedConfig.Theme);
            Assert.AreEqual("English", _viewModel.EditedConfig.Language);
        }

        [TestMethod]
        public void SaveChanges_SavesConfigToFile()
        {
            // Arrange
            _viewModel.EditedConfig.Theme = "Dark";
            _viewModel.EditedConfig.Language = "Vietnamese";

            // Act
            _viewModel.SaveChanges();

            // Assert
            Assert.IsTrue(File.Exists(_testFilePath), "File config không được tạo.");
            var savedConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_testFilePath));
            Assert.AreEqual("Dark", savedConfig.Theme);
            Assert.AreEqual("Vietnamese", savedConfig.Language);
        }

        [TestMethod]
        public void LoadSettings_FileExists_LoadsConfigCorrectly()
        {
            // Arrange
            var config = new Config
            {
                Theme = "Dark",
                Language = "Vietnamese"
            };
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(config));

            // Act
            var loadedConfig = AppearanceViewModel.LoadSettings();

            // Assert
            Assert.AreEqual("Dark", loadedConfig.Theme);
            Assert.AreEqual("Vietnamese", loadedConfig.Language);
        }
    }

    // Mock services for testing
    public class MockThemeSelectorService : IThemeSelectorService
    {
        private ElementTheme _currentTheme = ElementTheme.Light;

        public void SetTheme(ElementTheme theme)
        {
            _currentTheme = theme;
        }

        public ElementTheme GetTheme()
        {
            return _currentTheme;
        }
    }

    public class MockLanguageSelectorService : ILanguageSelectorService
    {
        private string _currentLanguage = "English";

        public string GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public void SetLanguage(string language)
        {
            _currentLanguage = language;
        }

        public event EventHandler LanguageChanged;
    }
}
