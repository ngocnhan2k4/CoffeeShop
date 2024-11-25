using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Helper;

public class SendEmailHelper
{
    public static void SendEmail(string recipientEmail, string message)
    {
       
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("tranngocnhannt2004@gmail.com", "lnci zfby anwh ikyd"),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("tranngocnhannt2004@gmail.com"),
            Subject = "Order Notification",
            Body = message,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(recipientEmail);

        smtpClient.Send(mailMessage);
    }
}
