using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public int customerID { get; set; }
        public string customerName { get; set; }
        public decimal totalMonney { get; set; }
        public double totalPoint { get; set; }
        public string type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Customer() {
            customerID = 0;
            customerName = "";
            totalMonney = 0;
            totalPoint = 0;
            type = "";
        }

        public Customer(int customerID, string customerName, decimal totalMonney, double totalPoint, string type)
        {
            this.customerID = customerID;
            this.customerName = customerName;
            this.totalMonney = totalMonney;
            this.totalPoint = totalPoint;
            this.type = type;
        }
    }
}
