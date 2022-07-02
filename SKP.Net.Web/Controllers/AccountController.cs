using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Core.Domain.Messages;
using SKP.Net.Services.Authentication;
using SKP.Net.Services.Customers;
using SKP.Net.Services.Messages;
using SKP.Net.Storage.Common;
using SKP.Net.Web.Models.Account;
using SKP.Net.Web.Utilities.Captcha;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKP.Net.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerService _customerService;
        private readonly StorageAccountConnection _storageAccountConnection;
        private readonly IMessageSender _messageSender;
        private Customer _customer;
        public AccountController(IAuthenticationService authenticationService,
            ICustomerService customerService,
            StorageAccountConnection storageAccountConnection,
            ICustomerRegistrationService customerRegistrationService,
            IMessageSender messageSender)
        {
            _authenticationService = authenticationService;
            _customerService = customerService;
            _customerRegistrationService = customerRegistrationService;
            _storageAccountConnection = storageAccountConnection;
            _messageSender = messageSender;
            _customer = _authenticationService.GetAuthenticatedCustomer();
        }

        [AllowAnonymous]
        [Route("Register")]
        public IActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                _customer = _authenticationService.GetAuthenticatedCustomer();
                if (_customer.OTP != model.OTP || string.IsNullOrEmpty(_customer.OTP))
                {
                    ModelState.AddModelError("Error", "Invalid OTP");
                    return View(model);
                }

                var customer = _customerService.GetCustomers().FirstOrDefault(arg => arg.Email.ToLower().Trim() == model.Email.ToLower().Trim() && arg.Active);
                if (customer != null)
                {
                    ModelState.AddModelError("Error", "User already exist");
                    return View(model);
                }

                _customer.Active = true;
                _customer.AdminComment = "Account Updated with registered role";
                _customer.UpdatedOnUtc = DateTime.UtcNow;
                _customer.Email = model.Email;
                _customer.Password = model.Password;
                _customer.OTP = string.Empty;
                _customer.OTPCount = 0;
                _customer.IsBlocked = false;
                _customerService.Update(_customer);
                _customerService.UpdateRole(_customer, RoleNames.RegisteredRole);
                await _messageSender.SendMesage(TemplateType.Register, _customer.Email, "Welcome", "Welcome your account is created successfully.");
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [AllowAnonymous]
        [Route("SignIn")]
        public async Task<IActionResult> Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _customer = _customerService.GetCustomers()
                    .FirstOrDefault(arg => arg.Email.ToLower().Trim() == model.Email.ToLower().Trim() && arg.Password == model.Password);
                if (_customer == null)
                {
                    ModelState.AddModelError("Error", "User name or password is incorrect.");
                    return View(model);
                }

                if (!_customer.Active)
                {
                    ModelState.AddModelError("Error", "Your account is not active");
                    return View(model);
                }

                if (_customer.IsBlocked)
                {
                    ModelState.AddModelError("Error", "Your account is blocked");
                    return View(model);
                }
                _authenticationService.SignIn(_customer, true);
                await _messageSender.SendMesage(TemplateType.Login, _customer.Email, "New Login", "Welcome back");
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                bool isAdminUser = _customerService.IsAdmin(_customer);
                if (isAdminUser)
                    return Redirect("/admin");
                return Redirect("/");

            }
            return View(model);
        }

        [Route("SignOut")]
        public IActionResult SignOut()
        {
            _authenticationService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Route("my-account")]
        public IActionResult MyAccount()
        {
            if (HasAccess())
            {
                var model = GetAccountModel();
                return View("Account",model); ;
            }
            return new UnauthorizedResult();
        }

        [Route("my-account")][HttpPost]
        public IActionResult MyAccount(AccountModel model)
        {
            if (HasAccess())
            {
                model = GetAccountModel();
                model.IsEditEnabled = true;
                model.IsLabel = true;
                if (!model.IsEditEnabled)
                {
                    model.IsEditEnabled = true;
                    model.IsLabel = false;
                }
                //model.IsEditEnabled = false;
                //model.IsLabel = true;
                return View("Account", model); 
            }
            return new UnauthorizedResult();
        }

        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }


        [Route("send-otp")]
        public async Task<IActionResult> SendOtp(string emailId)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            _customer = _authenticationService.GetAuthenticatedCustomer();
            bool isGuestRole = false;
            string otp = GenerateOTP();
            if (_customer != null)
            {
                isGuestRole = _customerService.IsGuestRole(_customer);
            }
            if (isGuestRole && !_customer.IsBlocked)
            {
                _customer.OTP = otp;
                _customer.OTPCount = _customer.OTPCount + 1;
                if (_customer.OTPCount > 3)
                {
                    _customer.Active = true;
                    _customer.IPAddress = ipAddress;
                    _customer.OTP = string.Empty;
                    _customer.OTPCount = 0;
                    _customer.IsBlocked = true;
                    _customerService.Update(_customer);
                    return Json("Account creation falied , due to maximum attempt.");
                }
                _customerService.Update(_customer);
                await _messageSender.SendMesage(TemplateType.OTP, emailId, "OTP Confirmation", otp);
                return Json("OTO Sent to your email Id.");
            }
            return Json("OTP send operation failed.");
        }

        public static string GenerateOTP()
        {
            var letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";
            Random rand = new Random();
            int maxRand = letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(letters[index]);
            }
            return sb.ToString();
        }

        private AccountModel GetAccountModel()
        {
            return new AccountModel
            {
                Email = this._customer.Email,
                Name = this._customer.Email,
                RowKey=this._customer.RowKey
                
            };
        }

        private bool HasAccess()
        {
            if (_customerService.IsAdmin(this._customer) || _customerService.IsRegisteredRole(this._customer) ||  _customerService.IsGuestRole(this._customer))
                return true;
            return false;
        }
    }
}
