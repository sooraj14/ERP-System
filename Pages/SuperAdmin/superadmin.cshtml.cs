using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.SuperAdmin
{
    public class superadminModel : PageModel
    {

        [BindProperty]
        public superadminlogin login { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if ((login.email.Equals("admin")) && (login.password.Equals("admin")))
            {
                return RedirectToPage("/SuperAdmin/Dashboard");
            }
            else
            {
                TempData["message"] = "Please enter valid credentials";
            }
            return Page();
        }
    }
}
