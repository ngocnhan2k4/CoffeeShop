using CoffeeShop.Service;
using CoffeeShop.Service.BusinessLogic;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopTests.Service.BusinessLogic
{
    [TestClass]
    public class SalesServiceTests
    {
       
        private SalesService salesService;

        [TestInitialize]
        public void Setup()
        {
            ServiceFactory.Register(typeof(IDao), typeof(SqlServerDao));
            var year = 2023;
            salesService = new SalesService(ServiceFactory.GetChildOf(typeof(IDao)) as IDao, year);
        }

        [TestMethod]
        public void TestSalesService_Initialization_ValidState()
        {
            Assert.IsNotNull(salesService.dao);
            Assert.IsTrue(salesService.TotalCost > 0);
            Assert.IsTrue(salesService.Revenue > 0);
            Assert.IsNotNull(salesService.Profit);
            CollectionAssert.AreEqual(new List<int>{ 2024,2023}, salesService.Years);
            Assert.IsNotNull(salesService.MonthlyRevenue);
            Assert.IsNotNull(salesService.TopDrinks);
            Assert.IsNotNull(salesService.RevenueByCategory);
            Assert.IsTrue(salesService.NumberOrders > 0);
        }
    }
}
