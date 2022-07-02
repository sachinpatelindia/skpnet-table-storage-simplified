using System;

namespace SKP.Net.Web.Models.Account
{
    public class AccountModel
    {
        public string RowKey { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime? DOB { get; set; }
        public string Profession { get; set; }
        public string Gender { get; set; }
        public string SocialMediaLink { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public bool IsLabel { get; set; }
        public bool IsEditEnabled { get; set; }
    }
}
