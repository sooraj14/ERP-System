using ERP_System.Data.context;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages
{
    public class PlansModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PlansModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public logincollge login { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPostBasic()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Basic only";

            _context.SaveChanges();
            return RedirectToPage("/Index");

        }
        public IActionResult OnPostStandard()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Standard Only";

            _context.SaveChanges();
            return RedirectToPage("/Index");
        }

        public IActionResult OnPostPremium()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Advanced Only";

            _context.SaveChanges();

            return RedirectToPage("/Index");


        }
    }
}
