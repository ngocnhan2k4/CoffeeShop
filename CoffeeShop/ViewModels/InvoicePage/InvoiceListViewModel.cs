using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.ViewModels.InvoicePage
{
    public class InvoiceListViewModel
    {
        public List<Invoice> invoices { get; set; }
        IDao _dao;
        public InvoiceListViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            invoices = _dao.GetInvoices();
        }
    }
}
