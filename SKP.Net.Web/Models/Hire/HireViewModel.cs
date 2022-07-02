using SKP.Net.Web.Areas.Admin.Models.Companies;
using System.Collections.Generic;

namespace SKP.Net.Web.Models.Hire
{
    public class HireViewModel
    {
        public HireViewModel()
        {
            this.Applications = new List<ApplicationModel>();
        }
        public string Email { get; set; }
        public string Message { get; set; }
        public CompanyViewModel Company { get; set; }
        public RequiredInformationModel RequiredInformation { get; set; }
        public List<ApplicationModel> Applications { get; set; }
    }
}
