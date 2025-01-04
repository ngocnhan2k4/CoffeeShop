using CoffeeShop.Helper;
using CoffeeShop.Models;
using CoffeeShop.Service;
using CoffeeShop.Service.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static CoffeeShop.Service.DataAccess.IDao;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CoffeeShop.ViewModels.HomePage
{

    public class HomeViewModel
    {
        public IDao _dao;
        public Invoice invoice;
        public ApiReq apiRequest;
        public readonly AccountSettings _accountSettings;
        public HomeViewModel()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "accountConfig.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _accountSettings = JsonConvert.DeserializeObject<AccountSettings>(json);
            }
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
        public string GetQrURL(bool isDelivery = false)
        {
            string accountNo = _accountSettings.AccountNo;
            string addInfo = Utilities.GenerateRandomString(8);
            apiRequest = new ApiReq
            {
                acqId = _accountSettings.BankCode,
                accountNo = Convert.ToInt64(accountNo),
                accountName = _accountSettings.AccountName,
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
