using ERP_System.Data.context;
using ERP_System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.SuperAdmin
{
    public class ClientViewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public ClientViewModel(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public List<CollegeAdmin> client { get; set; }

       
        public void OnGet()
        {
            client = _context.collegeadmins.ToList();
        }
    }
}
