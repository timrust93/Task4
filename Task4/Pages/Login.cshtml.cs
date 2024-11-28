using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Task4.AuthorizationHelpers;
using Task4.Models;
using Task4.Services;

namespace Task4.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginPoco Credential { get; set; } = new LoginPoco();

        private readonly RegistrationServices _registrationServices;

        public static string InvalidCredentialVD => ("InvalidCredentials");
        

        public LoginModel(RegistrationServices registrationServices)
        {
            _registrationServices = registrationServices;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dbUser = _registrationServices.FindRegisteredUser(Credential.Username, Credential.Password);

            if (dbUser == null)
            {                
                ViewData[InvalidCredentialVD] = "Incorrect username or password";
                return Page();
            }

            await HttpContext.SignInAsync(AuthHelper.AUTH_COOKIE, AuthHelper.GetClaimsPrincipal(dbUser));
            _registrationServices.UpdateLoginDate(dbUser);
            return RedirectToPage("AdminPanel");            
        }
    }
}
