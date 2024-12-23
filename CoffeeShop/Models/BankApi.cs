using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Models
{
    public class ApiReq
    {
        public long accountNo { get; set; }
        public string accountName { get; set; }
        public int acqId { get; set; }
        public int amount { get; set; }
        public string addInfo { get; set; }
        public string format { get; set; }
        public string template { get; set; }
    }

    public class Data
    {
        public int acpId { get; set; }
        public string accountName { get; set; }
        public string qrCode { get; set; }
        public string qrDataURL { get; set; }
    }

    public class ApiRes
    {
        public string code { get; set; }
        public string desc { get; set; }
        public Data data { get; set; }
    }

    public class Messages
    {
        public bool success { get; set; }
    }

    public class Transaction
    {
        public string id { get; set; }
        public string bank_brand_name { get; set; }
        public string account_number { get; set; }
        public string transaction_date { get; set; }
        public string amount_out { get; set; }
        public string amount_in { get; set; }
        public string accumulated { get; set; }
        public string transaction_content { get; set; }
        public string reference_number { get; set; }
        public object code { get; set; }
        public object sub_account { get; set; }
        public string bank_account_id { get; set; }
    }

    public class TransactionRes
    {
        public int status { get; set; }
        public object error { get; set; }
        public Messages messages { get; set; }
        public IList<Transaction> transactions { get; set; }
    }

}
