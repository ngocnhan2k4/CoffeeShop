using CoffeeShop.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CoffeeShop.Service.DataAccess.IDao;

namespace CoffeeShop.Service.DataAccess
{
    public class SqlServerDao : IDao
    {
        public List<Category> GetCategories()
        {
            var categories = new List<Category>();
            using var conn = new SqlConnection("Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;");
            conn.Open();

            using var cmd = new SqlCommand("SELECT name, id FROM category", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var category = new Category();
                category.CategoryName = reader.GetString(0);
                category.CategoryID = reader.GetInt32(1);
                categories.Add(category);
            }
            return categories;
        }

        public List<DeliveryInvoice> GetDeliveryInvoices()
        {
            var list = new List<DeliveryInvoice>();
            using var conn = new SqlConnection("Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;");
            conn.Open();

            using var cmd = new SqlCommand("SELECT * FROM delivery_invoice", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var di = new DeliveryInvoice();
                di.DeliveryInvoiceID = reader.GetInt32(0);
                di.Address = reader.GetString(1);
                di.PhoneNumber = reader.GetString(2);
                di.ShippingFee = reader.GetInt32(3);
                list.Add(di);
            }
            return list;
        }

        public List<DetailInvoice> GetDetailInvoices()
        {
            var list = new List<DetailInvoice>();
            using var conn = new SqlConnection("Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;");
            conn.Open();

            using var cmd = new SqlCommand("""
                        SELECT invoice_detail.drink_id, drink.name, invoice_detail.quantity, drink.size, invoice_detail.price
                        FROM invoice_detail,drink
                        WHERE invoice_detail.drink_id = drink.id
                        """, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var di = new DetailInvoice();
                di.InvoiceID = reader.GetInt32(0);
                di.NameDrink = reader.GetString(1);
                di.Quantity = reader.GetInt32(2);
                di.Size = reader.GetString(3);
                di.Price = reader.GetInt32(4);
            }
            return list;
        }

        public List<Drink> GetDrinks()
        {
            var drinks = new List<Drink>();
            using var conn = new SqlConnection("Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                            SELECT name,category_id,description,image FROM drink
                            GROUP BY name,category_id,description,image
                            """;
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var drink = new Drink
                {
                    Name = reader.GetString(0),
                    Description = reader.GetString(2),
                    ImageString = reader.GetString(3),
                    CategoryID = reader.GetInt32(1),
                    Discount = 0,
                    Sizes = new List<Size>()
                };

                var sizeCmd = conn.CreateCommand();
                sizeCmd.CommandText = "SELECT size, price, stock FROM drink WHERE name = @name GROUP BY size, price, stock ORDER BY size DESC";
                sizeCmd.Parameters.AddWithValue("@name", drink.Name);
                using var sizeReader = sizeCmd.ExecuteReader();
                while (sizeReader.Read())
                {
                    drink.Sizes.Add(new Size
                    {
                        Name = sizeReader.GetString(0),
                        Price = sizeReader.GetInt32(1),
                        Stock = sizeReader.GetInt32(2)
                    });
                }
                drinks.Add(drink);
            }
            return drinks;
        }

        public Tuple<List<Drink>, int> GetDrinks(int page, int rowsPerPage, string keyword, int categoryID, Dictionary<string, IDao.SortType> sortOptions)
        {
            var drinks = GetDrinks(); 
            // Search
            var query = from e in drinks
                        where e.Name.ToLower().Contains(keyword.ToLower())
                        select e;

            // Filter
            if (categoryID != -1)
            {
                query = query.Where(e => e.CategoryID == categoryID);
            }
            // Sort
            foreach (var option in sortOptions)
            {
                if (option.Key == "Price")
                {
                    if (option.Value == SortType.Ascending)
                    {
                        query = query.OrderBy(e => e.Sizes[0].Price * (1 - e.Discount));
                    }
                    else
                    {
                        query = query.OrderByDescending(e => e.Sizes[0].Price * (1 - e.Discount));
                    }
                }
                if (option.Key == "Stock")
                {
                    if (option.Value == SortType.Descending)
                    {
                        query = query.OrderByDescending(e => {
                            int sum = 0;
                            foreach (var size in e.Sizes)
                            {
                                sum += size.Stock;
                            }
                            return sum;
                        }
                        );
                    }
                }
            }

            var result = query
                .Skip((page - 1) * rowsPerPage)
                .Take(rowsPerPage);

            return new Tuple<List<Drink>, int>(
                result.ToList(),
                query.Count()
            );
        }

        public List<Invoice> GetInvoices()
        {
            var list = new List<Invoice>();
            using var conn = new SqlConnection("Server=127.0.0.1;Database=coffee-shop;User ID=sa;Password=SqlServer@123;TrustServerCertificate=True;");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM invoice";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var invoice = new Invoice();
                invoice.InvoiceID = reader.GetInt32(0);
                invoice.CreatedAt = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                invoice.TotalAmount = reader.GetInt32(2);
                invoice.PaymentMethod = reader.GetString(3);
                invoice.Status = reader.GetString(4);
                invoice.CustomerName = reader.GetString(5);
                invoice.HasDelivery = reader.GetString(6);
                list.Add(invoice);
            }
            return list;
        }
    }
}
