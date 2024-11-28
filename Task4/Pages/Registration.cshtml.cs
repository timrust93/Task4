using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Task4.Models;
using Task4.Services;

namespace Task4.Pages
{
    public class RegistrationModel : PageModel
    {
        [BindProperty]
        public UserRegisterModel UserRegisterModel { get; set; } = new UserRegisterModel();

        private readonly RegistrationServices _registrationServices;

        public static string RegistrationErrorVD => "RegistrationError";

        public RegistrationModel(RegistrationServices registrationServices)
        {
            _registrationServices = registrationServices;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool success = _registrationServices.RegisterUser(UserRegisterModel, out string errorMessage);

            if (success)
            {
                return RedirectToPage("Login");
            }
            else
            {
                ViewData[RegistrationErrorVD] = errorMessage;
                return Page();
            }
        }        
    }


}
