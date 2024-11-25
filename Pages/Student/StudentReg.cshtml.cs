using ERP_System.Data.context;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Student
{
    public class StudentRegModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public StudentRegModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public studentlogin students {  get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = _context.studentdetails.Where(s=> s.reg_num ==students.reg_no && s.password == students.pass).FirstOrDefault();
            HttpContext.Session.SetInt32("stud_id", user.student_id);
            return RedirectToPage("/Student/StudentDashboard");
        }

    }
}
