using CoffeeShop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace CoffeeShop.ViewModels.Settings
{
    public class PaymentSettingsViewModel : INotifyPropertyChanged
    {
        private AccountSettings _accountSettings;
        private AccountSettings _originalAccountSettings;
        private string filePath;
        public PaymentSettingsViewModel()
        {
            LoadSettings();
        }

        public AccountSettings AccountSettings
        {
            get => _accountSettings;
            set
            {
                _accountSettings = value;
                OnPropertyChanged();
            }
        }

        private void LoadSettings()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            filePath = Path.Combine(appDirectory, "accountConfig.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var x= JsonConvert.DeserializeObject(json);
                _originalAccountSettings = JsonConvert.DeserializeObject<AccountSettings>(json);
                AccountSettings = JsonConvert.DeserializeObject<AccountSettings>(json);
            }
            else
            {
                _originalAccountSettings = new AccountSettings();
                AccountSettings = new AccountSettings();
            }
        }

        public void ResetSettings()
        {
            AccountSettings = new AccountSettings{
                AccountName = _originalAccountSettings.AccountName,
                AccountNo = _originalAccountSettings.AccountNo,
                BankCode = _originalAccountSettings.BankCode,
                Token = _originalAccountSettings.Token
            };
        }

        public void SaveSettings()
        {
            _originalAccountSettings = JsonConvert.DeserializeObject<AccountSettings>(JsonConvert.SerializeObject(AccountSettings));
            var options = new JsonSerializerSettings { Formatting = Formatting.Indented };
            string json = JsonConvert.SerializeObject(_originalAccountSettings, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
