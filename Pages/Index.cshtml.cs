using ERP_System.Data.context;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public logincollge login {  get; set; }
        

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var collegeadmin = _context.collegeadmins.FirstOrDefault(c=>c.college_email==login.email && c.college_pass==login.password);
             HttpContext.Session.SetInt32("clge_admin", collegeadmin.college_id);
            return RedirectToPage("Plans");
        }
        public IActionResult OnPostBasic()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Basic Plan";

            _context.SaveChanges();
            return RedirectToPage("Index");
        }
        public IActionResult OnPostStandard()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Standard Plan";

            _context.SaveChanges();
            return RedirectToPage("Index");
        }

        public IActionResult OnPostPremium()
        {
            int? id = HttpContext.Session.GetInt32("clge_admin");
            var details = _context.collegeadmins.Where(cd => cd.college_id == (int)id).FirstOrDefault();
            details.subscription = "Premium Plan";

            _context.SaveChanges();
            return RedirectToPage("Index");
        }

    }
}
