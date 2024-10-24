using CoffeeShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.DataAccess
{
    /// <summary>
    /// This class represents a mock data access object
    /// </summary>
    public class MockDao : IDao
    {
        public List<Category> GetCategories()
        {
            var list = new List<Category>()
           {
               new Category() { CategoryID = 96, CategoryName = "Trà sữa" },
               new Category() { CategoryID = 97, CategoryName = "Cà phê" },
               new Category() { CategoryID = 98, CategoryName = "Trà trái cây tươi" },
               new Category() { CategoryID = 99, CategoryName = "Sinh tố" },
               new Category() { CategoryID = 100, CategoryName = "Nước ép" }
           };
           return list;
        }

        public List<DeliveryInvoice> GetDeliveryInvoices()
        {
            var list = new List<DeliveryInvoice>()
            {
                new DeliveryInvoice() { DeliveryInvoiceID = 1, PhoneNumber = "0123456789", ShippingFee = 5000, Address="KTX khu B" },
               
            };
            return list;
        }

        public List<DetailInvoice> GetDetailInvoices()
        {
            var list = new List<DetailInvoice>()
            {
                new DetailInvoice() { InvoiceID = 1, NameDrink = "Cà phê sữa", Quantity = 1, Size = "M" },
                new DetailInvoice() { InvoiceID = 2, NameDrink = "Cà phê sữa", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 3, NameDrink = "Trà sữa matcha", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 4, NameDrink = "Cà phê sữa", Quantity = 1, Size = "S" },
                new DetailInvoice() { InvoiceID = 5, NameDrink = "Sinh tố xoài", Quantity = 1, Size = "S" },
            };
            return list;
        }

        public List<Drink> GetDrinks()
        {
            var list = new List<Drink>()
    {
        new Drink() {Image="/Assets/images.jpeg" , Name="Cà phê sữa Cà phê sữa Cà phê sữa", Size="S", Description="cà phê có sữa", OriginalPrice=12000, SalePrice=15000, Stock=100, CategoryID=97 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Cà phê sữa", Size="M", Description="cà phê có sữa", OriginalPrice=15000, SalePrice=18000, Stock=100, CategoryID=97 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Cà phê sữa", Size="L", Description="cà phê có sữa", OriginalPrice=18000, SalePrice=21000, Stock=100, CategoryID=97 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Cà phê đen", Size="S", Description="cà phê không sữa", OriginalPrice=10000, SalePrice=12000, Stock=100, CategoryID=97 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Trà sữa trân châu", Size="S", Description="chỉ có trân châu đen", OriginalPrice=18000, SalePrice=20000, Stock=100, CategoryID=96 },
        new Drink() {Image="/Assets/images.jpeg",  Name="Trà sữa matcha", Size="S", Description="có matcha", OriginalPrice=25000, SalePrice=27000, Stock=100, CategoryID=96 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Trà sữa hoa hồng", Size="S", Description="có hoa hồng", OriginalPrice=25000, SalePrice=27000, Stock=100, CategoryID=96 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Sinh tố dâu", Size="S", Description="sinh tố làm từ dâu", OriginalPrice=18000, SalePrice=27000, Stock=100, CategoryID=99 },
        new Drink() {Image="/Assets/images.jpeg",  Name="Sinh tố xoài", Size="S", Description="sinh tố làm từ xoài", OriginalPrice=18000, SalePrice=27000, Stock=100, CategoryID=99 },
        new Drink() {Image="/Assets/images.jpeg" ,  Name="Sinh tố đu đủ", Size="S", Description="sinh tố làm từ đu đủ", OriginalPrice=18000, SalePrice=27000, Stock=100, CategoryID=99 }
    };

            return list;
        }

        public List<Invoice> GetInvoices()
        {
            var list = new List<Invoice>()
            {
                new(){InvoiceID=1, CreatedAt="2024-10-16",TotalAmount=18000, PaymentMethod="Credit Card", CustomerName="Nguyễn Văn A", Status="Paid"},
                new(){InvoiceID=3, CreatedAt="2024-10-15",TotalAmount=27000, PaymentMethod="Bank Transfer", CustomerName="Phạm Văn C", Status="Pending"},
                new(){InvoiceID=2, CreatedAt="2024-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="UnPaid"},
                new(){InvoiceID=4, CreatedAt="2023-10-16",TotalAmount=15000, PaymentMethod="Cash", CustomerName="Trần Thị B", Status="UnPaid"},
                new(){InvoiceID=5, CreatedAt="2023-9-16",TotalAmount=27000, PaymentMethod="Cash", CustomerName="Trần Thị C", Status="UnPaid"}
            };
            return list;


        }
    }
}
