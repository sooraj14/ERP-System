using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace ERP_System.Pages.Admin
{
    public class AdminSignupModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public SignupClass adminsignup { get; set; }
        public AdminSignupModel(ApplicationDbContext dbcontext)
        {
            _context = dbcontext;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                
                    TempData["messageLoginfailed"] = "Signup Failed";
                    return Page();
            }
            var ca = new CollegeAdmin
            {
                college_id = adminsignup.college_refno,
                college_name = adminsignup.college_name,
                college_email = adminsignup.college_email,

                college_pass = adminsignup.college_password,
                purchased = DateTime.Now
            };
            _context.collegeadmins.Add(ca);
            _context.SaveChanges();
            return RedirectToPage("/Admin/AdminLogin");
        }


    }
        
}
