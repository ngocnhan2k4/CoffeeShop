using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels.HomePage
{
    public class ChoseDrinkViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public FullObservableCollection<DetailInvoice> ChosenDrinks { get; set; }
        public Invoice Invoice { get; set; }
        public DeliveryInvoice DeliveryInvoice { get; set; }
        public int TotalPrice { get; set; }

        public int Discount { get; set; } = 0;
        public int TotalPriceAfterDiscount { get; set; }
        public int CustomerId { get; set; } = 0;

        public List<MemberCard> memberCards { get; set; }
        public ChoseDrinkViewModel()
        {
            ChosenDrinks = new FullObservableCollection<DetailInvoice>();
            memberCards = GetMemberCards();
        }
        public void AddDrink(Drink drink, Size size)
        {
            if(size.Stock == 0) return;
            var existingInvoice = ChosenDrinks.FirstOrDefault(di => di.NameDrink == drink.Name && di.Size == size.Name);
            if (existingInvoice != null)
            {
                if (existingInvoice.Quantity >= size.Stock) return;
                existingInvoice.Quantity += 1;
            }
            else
            {
                var newInvoice = new DetailInvoice
                {
                    DrinkId = size.ID,
                    NameDrink = drink.Name,
                    Quantity = 1,
                    MaxQuantity= size.Stock,
                    Size = size.Name,
                    Price = (int)drink.GetDiscountedPrice(size)
                };
                ChosenDrinks.Add(newInvoice);
            }
            CalcTotal();
        }
        public void RemoveDrink(DetailInvoice detail)
        {
            ChosenDrinks.Remove(detail);
            CalcTotal();
        }
        public void CalcTotal()
        {
            TotalPrice = ChosenDrinks.Sum(di => di.Price * di.Quantity);
            if (CustomerId == 0)
            {
                TotalPriceAfterDiscount = TotalPrice;
                return;
            }
            List<Customer>  customers = GetCustomers();
            Customer customer = customers.FirstOrDefault(c => c.customerID == CustomerId);
            if(customer==null)
            {
                Discount = 0;
                TotalPriceAfterDiscount = TotalPrice;
                return;
            }
            //if (customer.type == "Thẻ thành viên")
            //    Discount = (TotalPrice * 5) / 100;
            //else if (customer.type == "Thẻ bạc")
            //    Discount = (TotalPrice * 10) / 100;
            //else if (customer.type == "Thẻ vàng")
            //    Discount = (TotalPrice * 15) / 100;

            Discount = (TotalPrice * memberCards.FirstOrDefault(m => m.CardName == customer.type).Discount) / 100;

            TotalPrice -= Discount;
            TotalPriceAfterDiscount = TotalPrice;

        }
        internal Invoice AddInvoice(Invoice invoice, DeliveryInvoice delivery)
        {
            IDao _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            return _dao.AddInvoice(invoice, ChosenDrinks.ToList(), delivery, CustomerId);

        }

        public List<Customer> GetCustomers()
        {
            IDao _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            return _dao.GetCustomers();
        }

        public void SetCustomerId(int memberId)
        {
            CustomerId = memberId;
            CalcTotal();
        }

        public List<MemberCard> GetMemberCards()
        {
            IDao _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            return _dao.GetMemberCards();
        }
    }
}
