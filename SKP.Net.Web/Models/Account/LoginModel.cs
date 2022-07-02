using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Models.Account
{
    public class LoginModel
    {
        [Display(Name = "Email"), Required,EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password"), DataType(DataType.Password), Required, MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
