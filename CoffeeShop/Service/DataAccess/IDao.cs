using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CoffeeShop.Service.DataAccess
{
    public interface IDao
    {
        public enum SortType
        {
            Ascending,
            Descending
        }
        List<Drink> GetDrinks();
        public Tuple<List<Drink>, int> GetDrinks(
            int page, int rowsPerPage,
            string keyword, int categoryID,
            Dictionary<string, SortType> sortOptions
        );
        public bool AddDrinks(List<Drink> drinks);
        List<Category> GetCategories();
        public bool AddCategories(List<Category> categories);
        public List<Discount> GetDiscounts();
        public bool AddDiscounts(List<Discount> discounts);
        List<DeliveryInvoice> GetDeliveryInvoices();
        List<Invoice> GetInvoices();
        List<DetailInvoice> GetDetailInvoices();

        public int CalculateNumberOrders(int year);
        public int CalculateTotalCost();
        public int CalculateRevenue(int year);
        public int CalculateProfit(int year);
        public List<int> CalculateYears();
        public List<int> CalculateMonthlyRevenue(int year);
        public List<string> CalculateTopDrinks(int year);
        public Dictionary<string, int> CalculateRevenueCategory(int year);

        public Tuple<List<DetailInvoice>, DeliveryInvoice>  GetDetailInvoicesOfId(int invoiceId);
        public void UpdateInvoiceStatus(int invoiceId, string status);
        public Invoice AddInvoice(Invoice invoice, List<DetailInvoice> detailInvoices,DeliveryInvoice deliveryInvoice);

        public List<Invoice> GetRecentInvoice(int year); // Get recent 5 invoices
    }
}
