using System.ComponentModel.DataAnnotations;

namespace SKP.Net.Web.Models.Account
{
    public class RegisterModel
    {
        [Display(Name = "Email"), Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password"), DataType(DataType.Password), Required, MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password"), DataType(DataType.Password), Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [StringLength(4), DataType(DataType.Text)]
        public string OTP { get; set; }
    }
}
