using SKP.Net.Core.Domain.Messages;
using SKP.Net.Storage.Operations;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SKP.Net.Services.Messages
{
    public class MessageSender : IMessageSender
    {
        private readonly IEmailAccountService _emailAccountService;
        private readonly ITableStorage<EmailTemplate> _emailTemplateStorage;
        public MessageSender(IEmailAccountService emailAccountService,
            ITableStorage<EmailTemplate> emailTemplateStorage)
        {
            _emailAccountService = emailAccountService;
            _emailTemplateStorage = emailTemplateStorage;
        }
        public async Task SendMesage(TemplateType templateType, string toEmail, string subject, string message)
        {
            var template = _emailTemplateStorage.GetAll<EmailTemplate>().Where(m=>m.TemplateType==templateType).FirstOrDefault();
            var templateBody = template.Template.Replace("(#)",""+message+"");
            var emailAccount = _emailAccountService.GetDefaultEmailAccount();
            var mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress(emailAccount.From, emailAccount.DisplayName);
            mailMessage.To.Add(new MailAddress(toEmail));
            mailMessage.Subject = subject;
            mailMessage.Body = templateBody+ message;
            mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            using (var client = new SmtpClient(emailAccount.Host, emailAccount.Port))
            {
                client.Credentials = new NetworkCredential(emailAccount.SmtpUserName, emailAccount.Password);
                client.EnableSsl = emailAccount.EnableSsl;

                try
                {
                  await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {

                }

            }
        }
        private string GetTemplate(string message)
        {
            var newstring = message.Replace("%%",""+message+"");
            return newstring;
        }
    }
}
