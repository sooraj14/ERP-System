using ERP_System.Data.context;
using ERP_System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.SuperAdmin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public int count { get; set; }
        public List<CollegeAdmin> client { get; set; }


        public void OnGet()
        {
            count = _context.collegeadmins.Count();
            client = _context.collegeadmins.ToList();
        }
        public IActionResult OnPostBlock(int id)
        {
            var user = _context.collegeadmins.Find(id);
            if (user != null)
            {
                user.Active = false;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
        public IActionResult OnPostUnblock(int id)
        {
            var user = _context.collegeadmins.Find(id);
            if (user != null)
            {
                user.Active = true;
                _context.SaveChanges();
            }
            return RedirectToPage();
        }
       
    }
}
