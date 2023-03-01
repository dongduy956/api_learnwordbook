using API.COMMON.Configs;
using API.SERVICES.IServices;
using API.SERVICES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace API.SERVICES.Services
{
    public class MailServices : IMailServices
    {

        public async Task<bool> SendMail(Mail mail)
        {
            //gán thông tin
            var mess = new MailMessage(MailConfigs.Mail, mail.MailTo);
            mess.Subject = mail.Title;
            mess.Body = mail.Body;
            //cho gửi định dạng html
            mess.IsBodyHtml = true;
            //cấu hình mail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            //gửi mail đi
            NetworkCredential net = new NetworkCredential(MailConfigs.Mail, MailConfigs.Pass);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = net;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                await smtp.SendMailAsync(mess);
            }
            catch (SmtpException ex)
            {
                return false;
            }
            return true;
        }
    }
}
