using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoffeeShop.ViewModels.InvoicePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeShop.Models;
using CoffeeShop.Service.DataAccess;
using CoffeeShop.Service;
using Microsoft.UI.Xaml;

namespace CoffeeShop.ViewModels.InvoicePage.Tests
{
    [TestClass()]
    public class InvoiceDetailViewModelTests
    {
        private InvoiceDetailViewModel _viewModel;
        Invoice _invoice = new Invoice()
        {
            InvoiceID = 1,
            Status = "Wait"
        };
        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            _viewModel = new InvoiceDetailViewModel();
        }

        [TestMethod]
        public void InvoiceDetailViewModel_Constructor_ShouldInitializeProperties()
        {
            Assert.IsNotNull(_viewModel.detailInvoices);
            Assert.IsNull(_viewModel._invoice);
        }

        [TestMethod]
        public void SetDetailInvoices_ShouldSetDetailInvoices()
        {
            IDao _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            var tmp = _dao.GetDetailInvoicesOfId(_invoice.InvoiceID);
            _viewModel.SetDetailInvoices(_invoice);
            Assert.AreEqual(_invoice, _viewModel._invoice);
            Assert.AreEqual(tmp.Count, _viewModel.detailInvoices.Count);
            Assert.AreEqual(tmp[0].NameDrink, _viewModel.detailInvoices[0].NameDrink);
        }

        [TestMethod]
        public void ButtonVisibility_ShouldReturnVisible_WhenStatusIsWait()
        {
            _viewModel.SetDetailInvoices(_invoice);
            Assert.AreEqual(Visibility.Visible, _viewModel.ButtonVisibility);
        }

        [TestMethod]
        public void ButtonVisibility_ShouldReturnCollapsed_WhenStatusIsNotWait()
        {
            _invoice.Status = "Paid";
            _viewModel.SetDetailInvoices(_invoice);
            Assert.AreEqual(Visibility.Collapsed, _viewModel.ButtonVisibility);
        }

        [TestMethod]
        public void SetStatus_ShouldUpdateStatusAndNotifyPropertyChanged()
        {
            bool propertyChangedFired = false;
            _viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "ButtonVisibility")
                {
                    propertyChangedFired = true;
                }
            };
            _viewModel.SetDetailInvoices(_invoice);
            _viewModel.setStatus("Paid");
            Assert.AreEqual("Paid", _viewModel._invoice.Status);
            Assert.IsTrue(propertyChangedFired);
        }
    }
}