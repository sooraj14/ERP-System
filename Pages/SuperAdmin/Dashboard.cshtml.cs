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
        public void OnGet()
        {
           count = _context.collegeadmins.Count();
            
        }
    }
}
