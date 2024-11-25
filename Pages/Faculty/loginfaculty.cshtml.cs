using ERP_System.Data.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Faculty
{
    public class loginfacultyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public loginfacultyModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string FacEmail { get; set; } // Faculty email input

        [BindProperty]
        public string FacPassword { get; set; } 

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(FacEmail) || string.IsNullOrEmpty(FacPassword))
            {
                TempData["ErrorMessage"] = "Email and password are required!";
                return Page();
            }

            var faculty = _context.facultydetails
                .FirstOrDefault(f => f.fac_email == FacEmail && f.fac_password == FacPassword);

            if (faculty == null)
            {
                TempData["ErrorMessage"] = "Invalid email or password!";
                return Page();
            }

            HttpContext.Session.SetInt32("FacultyID", faculty.fac_id);

            return RedirectToPage("/Faculty/FacultyDashboard");
        }
    }
}
