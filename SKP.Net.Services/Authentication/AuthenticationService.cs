using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SKP.Net.Core.Domain.Customers;
using SKP.Net.Services.Customers;
using System.Linq;
namespace SKP.Net.Services.Authentication
{
    public partial class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerService _customerService;
        private Customer _cachedCustomer;
        public AuthenticationService(IHttpContextAccessor httpContextAccessor,
            ICustomerService customerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _customerService = customerService;
        }

        public Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;

            var authenticateResult = _httpContextAccessor.HttpContext.AuthenticateAsync(SkpAuthenticationDefaults.AuthenticationScheme).Result;
            if (!authenticateResult.Succeeded)
                return null;

            var emailClaim = authenticateResult.Principal.FindFirst(m => m.Type == ClaimTypes.Name || m.Type == ClaimTypes.Email && m.Issuer
            .Equals(SkpAuthenticationDefaults.ClaimIssuer, StringComparison.InvariantCultureIgnoreCase) && m.Type==ClaimTypes.UserData);

            var userClaim = authenticateResult.Principal.FindFirst(m => m.Issuer.Equals(SkpAuthenticationDefaults.ClaimIssuer, StringComparison.InvariantCultureIgnoreCase) && m.Type == ClaimTypes.UserData);
            Customer customer = null;

            if (emailClaim != null)
                customer = _customerService.GetCustomers().FirstOrDefault(m => m.RowKey.Equals(userClaim.Value) && m.Email.Contains(emailClaim.Value));
            if (customer == null || !customer.Active)
                return null;
            _cachedCustomer = customer;
            return _cachedCustomer;
        }

        public async void SignIn(Customer customer, bool isPersistent)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(customer.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, customer.Email, ClaimTypes.Email, SkpAuthenticationDefaults.ClaimIssuer));
                claims.Add(new Claim(ClaimTypes.Name, customer.Email, ClaimTypes.Name, SkpAuthenticationDefaults.ClaimIssuer));
                claims.Add(new Claim(ClaimTypes.UserData, customer.RowKey, ClaimTypes.UserData, SkpAuthenticationDefaults.ClaimIssuer));
            }

            var userIdentity = new ClaimsIdentity(claims, SkpAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);
            var authenticationProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
                IsPersistent = isPersistent
            };

            await _httpContextAccessor.HttpContext.SignInAsync(SkpAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);
            _cachedCustomer = customer;
        }
        public async void SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(SkpAuthenticationDefaults.AuthenticationScheme);
        }
    }
}