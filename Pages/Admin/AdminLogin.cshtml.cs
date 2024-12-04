using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Admin
{
    public class AdminLoginModel : PageModel
    {

        public readonly ApplicationDbContext _context;
        [BindProperty]

        public AdminLogincs adminlogin { get; set; }

       

        public AdminLoginModel(ApplicationDbContext context)
        {
            _context = context;

        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {

            if (ModelState.IsValid)
            {
                var admincheck = _context.collegeadmins.Where(p => p.college_email == adminlogin.college_email && p.college_pass == adminlogin.college_password).FirstOrDefault();

                if (admincheck != null)
                {
                    HttpContext.Session.SetInt32("college_id", admincheck.college_id);
                    TempData["isactive"] = admincheck.subscription;
                    return RedirectToPage("/Admin/EmptyPage");
                }

                else
                {
                    TempData["messageLoginfailed"] = "Login Failed";
                    return RedirectToPage();
                }
            }

            return Page();
            
        }
    }
}
