using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Messages;
using SKP.Net.Services.Messages;
using SKP.Net.Web.Areas.Admin.Models.Messages;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class EmailAccountController : BaseAdminController
    {
        private readonly IEmailAccountService _emailAccountService;
        public EmailAccountController(IEmailAccountService emailAccountService)
        {
            _emailAccountService = emailAccountService;
        }

        public IActionResult Index()
        {
            if (!HasDefaultEmailAccount())
                return RedirectToAction("Create");
            var email = _emailAccountService.GetDefaultEmailAccount();
            var model = new EmailAccountModel
            {
                DisplayName = email.DisplayName,
                EnableSsl = email.EnableSsl,
                From = email.From,
                Host = email.Host,
                Password = email.Password,
                Port = email.Port,
                SmtpUserName = email.SmtpUserName,
                RowKey=email.RowKey
            };
            return View(model);
        }

        public IActionResult Create()
        {
            if (HasDefaultEmailAccount())
                return RedirectToAction("Index");
            var model = new EmailAccountModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EmailAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var email = new EmailAccount
                {
                    DisplayName = model.DisplayName,
                    EnableSsl = model.EnableSsl,
                    From = model.From,
                    Host = model.Host,
                    Password = model.Password,
                    Port = model.Port,
                    SmtpUserName = model.SmtpUserName
                };

                _emailAccountService.Insert(email);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(string rowKey)
        {
            if (!HasDefaultEmailAccount())
                return RedirectToAction("Create");
            var emailAccount = _emailAccountService.GetDefaultEmailAccount();

            var model = new EmailAccountModel
            {
                DisplayName = emailAccount.DisplayName,
                EnableSsl = emailAccount.EnableSsl,
                From = emailAccount.From,
                Host = emailAccount.Host,
                Password = emailAccount.Password,
                Port = emailAccount.Port,
                RowKey = emailAccount.RowKey,
                SmtpUserName = emailAccount.SmtpUserName
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmailAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var emailAccount = _emailAccountService.GetDefaultEmailAccount();

                emailAccount.DisplayName = model.DisplayName;
                emailAccount.EnableSsl = model.EnableSsl;
                emailAccount.From = model.From;
                emailAccount.Host = model.Host;
                emailAccount.Password = model.Password;
                emailAccount.Port = model.Port;
                emailAccount.SmtpUserName = model.SmtpUserName;

                _emailAccountService.Update(emailAccount);
                return RedirectToAction("Index");
            }

            return View(model);
        }


        [NonAction]
        private bool HasDefaultEmailAccount()
        {
            if (_emailAccountService.GetDefaultEmailAccount() == null)
                return false;
            return true;
        }
    }
}
