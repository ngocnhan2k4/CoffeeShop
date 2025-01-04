using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.ViewModels.Settings;
using CoffeeShop.Models;
using System.IO;
using Newtonsoft.Json;
using System;

namespace CoffeeShopTests.ViewModels.Settings
{
    [TestClass]
    public class PaymentSettingsViewModelTests
    {
        private PaymentSettingsViewModel _viewModel;
        private string _testFilePath;

        [TestInitialize]
        public void Setup()
        {
            _testFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accountConfig.json");
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            _viewModel = new PaymentSettingsViewModel();
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
        public void LoadSettings_FileExists_InitializesPropertiesCorrectly()
        {
            // Arrange
            var accountSettings = new AccountSettings
            {
                AccountName = "Test Account",
                AccountNo = "123456",
                BankCode = 12345,
                Token = "token123"
            };
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(accountSettings, Newtonsoft.Json.Formatting.Indented));

            // Act
            _viewModel = new PaymentSettingsViewModel();

            // Assert
            Assert.AreEqual("Test Account", _viewModel.AccountSettings.AccountName);
            Assert.AreEqual("123456", _viewModel.AccountSettings.AccountNo);
            Assert.AreEqual(12345, _viewModel.AccountSettings.BankCode);
            Assert.AreEqual("token123", _viewModel.AccountSettings.Token);
        }

        [TestMethod]
        public void ResetSettings_ResetsPropertiesToOriginalValues()
        {
            // Arrange
            var originalSettings = new AccountSettings
            {
                AccountName = "Original Account",
                AccountNo = "654321",
                BankCode = 12345,
                Token = "token456"
            };
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(originalSettings));
            _viewModel = new PaymentSettingsViewModel();
            _viewModel.AccountSettings.AccountName = "Modified Account";

            // Act
            _viewModel.ResetSettings();

            // Assert
            Assert.AreEqual("Original Account", _viewModel.AccountSettings.AccountName);
            Assert.AreEqual("654321", _viewModel.AccountSettings.AccountNo);
            Assert.AreEqual(12345, _viewModel.AccountSettings.BankCode);
            Assert.AreEqual("token456", _viewModel.AccountSettings.Token);
        }

        [TestMethod]
        public void SaveSettings_SavesCurrentSettingsToFile()
        {
            // Arrange
            var accountSettings = new AccountSettings
            {
                AccountName = "Test Account",
                AccountNo = "123456",
                BankCode = 12345,
                Token = "token123"
            };
            _viewModel.AccountSettings = accountSettings;

            // Act
            _viewModel.SaveSettings();

            // Assert
            var savedSettings = JsonConvert.DeserializeObject<AccountSettings>(File.ReadAllText(_testFilePath));
            Assert.AreEqual("Test Account", savedSettings.AccountName);
            Assert.AreEqual("123456", savedSettings.AccountNo);
            Assert.AreEqual(12345, savedSettings.BankCode);
            Assert.AreEqual("token123", savedSettings.Token);
        }
    }
}