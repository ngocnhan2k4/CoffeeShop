using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class AccountSettings : INotifyPropertyChanged
    {
        private string _accountNo;
        private string _accountName;
        private string _token;
        private int _bankCode;
        public string AccountNo
        {
            get => _accountNo;
            set
            {
                _accountNo = value;
                OnPropertyChanged();
            }
        }

        public string AccountName
        {
            get => _accountName;
            set
            {
                _accountName = value;
                OnPropertyChanged();
            }
        }

        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                OnPropertyChanged();
            }
        }

        public int BankCode
        {
            get => _bankCode;
            set
            {
                _bankCode = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
