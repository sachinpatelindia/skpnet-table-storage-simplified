using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Messages;
using SKP.Net.Storage.Operations;
using SKP.Net.Web.Areas.Admin.Models.Messages;
using System.Linq;

namespace SKP.Net.Web.Areas.Admin.Controllers
{
    public class EmailTemplateController : BaseAdminController
    {
        private readonly ITableStorage<EmailTemplate> _emailTemplateStorage;
        public EmailTemplateController(ITableStorage<EmailTemplate> emailTemplateStorage)
        {
            _emailTemplateStorage = emailTemplateStorage;
        }
        public IActionResult Index()
        {
            var template = _emailTemplateStorage.GetAll<EmailTemplate>().Select(m => new EmailTemplateModel
            {
                Active = m.Active,
                RowKey = m.RowKey,
                Template = m.Template,
                TemplateType=(TemplateType)m.TemplateTypeId              
            });
            return View(template);
        }


        public IActionResult Create()
        {
            var model = new EmailTemplateModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(EmailTemplateModel model)
        {
            if(ModelState.IsValid)
            {
                var existing = _emailTemplateStorage.GetAll<EmailTemplate>().Where(m=>m.TemplateType==model.TemplateType);
                if (existing.Count() > 0)
                {
                    ModelState.AddModelError("error", "Template alredy exist");
                    return View(model);
                }

                _emailTemplateStorage.Insert(new EmailTemplate
                {
                    Active = model.Active,
                    Template = model.Template,
                    TemplateType = model.TemplateType,
                    TemplateTypeId = model.TemplateTypeId
                });

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
