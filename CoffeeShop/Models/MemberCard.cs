using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class MemberCard : INotifyPropertyChanged
    {
        public string CardName { get; set; }
        public int Discount { get; set; }

        public MemberCard(string cardName, int discount)
        {
            CardName = cardName;
            Discount = discount;
        }

        public MemberCard()
        {
            CardName = "";
            Discount = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
