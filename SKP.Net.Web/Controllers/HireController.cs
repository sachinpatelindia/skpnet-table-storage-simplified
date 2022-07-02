using Microsoft.AspNetCore.Mvc;
using SKP.Net.Web.Areas.Admin.Models.Companies;
using SKP.Net.Web.Models.Hire;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SKP.Net.Web.Controllers
{
    public class HireController : BaseController
    {
        public IActionResult Index()
        {
            var model = new HireViewModel();
            model.Company = new CompanyViewModel { Active = false };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(HireViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return View(model);

            var names = model.Email.Split("@");
            var company = GetCompanies().Where(arg => arg.Email.Contains(model.Email) || arg.DomainName.Equals(names[1])).SingleOrDefault();
            if (company == null)
            {
                model.Company = new CompanyViewModel { Active = false };
                ModelState.AddModelError("error", "Invaid Company");
                return View(model);
            }
            model.Company = company;
            model.Applications = GetApplications();
            return View(model);
        }

        [HttpPost]
        public IActionResult Request(HireViewModel model)
        {
            return RedirectToAction("Index");
        }
        private List<ApplicationModel> GetApplications()
        {
            return new List<ApplicationModel>
            {
                new ApplicationModel
                {
                     AppliedOn=DateTime.Now.AddDays(11),
                      ContactPerson="KK 1",
                       ContactPersonEmail="KK1@ibm.com",
                        Location="Bangalore",
                         PositionName="TL"
                },
                  new ApplicationModel
                {
                     AppliedOn=DateTime.Now.AddDays(11),
                      ContactPerson="KK BB",
                       ContactPersonEmail="KKBB@ibm.com",
                        Location="Delhi",
                         PositionName=".Net"
                }
            };
        }
        private List<CompanyViewModel> GetCompanies()
        {
            return new List<CompanyViewModel>
            {
                new CompanyViewModel
                {
                     Name="Microsoft",
                     Email="hr@microsoft.com",
                     DomainName="Microsoft.com",
                     Active=true,
                     Phone="080123456",
                     Url="careers@microsoft.com"
                },
                 new CompanyViewModel
                {
                     Name="IBM",
                     Email="hr@ibm.com",
                     DomainName="ibm.com",
                     Active=true,
                     Phone="080123456",
                     Url="careers@ibm.com"
                },
                    new CompanyViewModel
                {
                     Name="Invalid Company",
                     Email="invalid@gmail.com",
                     DomainName="gmail.com",
                     Active=false,
                     Phone="080123456",
                     Url="invalid.gmail.com"
                }
            };
        }
    }
}
