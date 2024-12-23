using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CoffeeShop.Service.DataAccess.IDao;

namespace CoffeeShop.ViewModels.HomePage
{
    public class HomeViewModel
    {
        public IDao _dao;
        public Invoice invoice;
        public string accountNo = "0915680152";
        public ApiReq apiRequest;
        public HomeViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
        }
        public bool checkTThai(int id)
        {
            //get the current year
            int year = DateTime.Now.Year;
            var dataResult = _dao.GetRecentInvoice(year);
            foreach (var invoice in dataResult)
            {
                if (invoice.InvoiceID == id && invoice.Status == "Cancel") return true;
            }
            return false;

        }
        public void changeTThai(Invoice _invoice)
        {
            _dao.UpdateInvoiceStatus(_invoice.InvoiceID, "Paid");
        }
        public ApiReq CreateApiReq(string _addInfo) {
            apiRequest = new ApiReq
            {
                acqId = 970422,
                accountNo = Convert.ToInt64(accountNo),
                accountName = "NGUYEN DINH MINH NHAT",
                addInfo = _addInfo,
                amount = invoice.TotalAmount,
                format = "text",
                template = "print"
            };
            return apiRequest;
        }
        public string GetQrURL(bool isDelivery = false)
        {
            string addInfo = Utilities.GenerateRandomString(3);
            apiRequest = new ApiReq
            {
                acqId = 970422,
                accountNo = Convert.ToInt64(accountNo),
                accountName = "NGUYEN DINH MINH NHAT",
                addInfo = addInfo,
                amount = isDelivery ? invoice.TotalAmount+10000 : invoice.TotalAmount,
                format = "text",
                template = "print"
            };
            // Generate the QR code image
            string jsonString = JsonSerializer.Serialize(apiRequest);

            // Check if accountNo length in jsonString is less than the original accountNo string
            if (jsonString.Contains($"\"accountNo\":{apiRequest.accountNo}") && apiRequest.accountNo.ToString().Length < accountNo.Length)
            {
                jsonString = jsonString.Replace($"\"accountNo\":{apiRequest.accountNo}", $"\"accountNo\":\"{accountNo}\"");
            }
            string qrCodeUrl = $"https://img.vietqr.io/image/{apiRequest.acqId}-{accountNo}-{apiRequest.template}.png?amount={apiRequest.amount}&addInfo={apiRequest.addInfo}&accountName={Uri.EscapeDataString(apiRequest.accountName)}";
            return qrCodeUrl;
        }
    }
}
