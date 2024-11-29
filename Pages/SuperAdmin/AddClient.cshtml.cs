using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.SuperAdmin
{
    public class AddClientModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AddClientModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]

        public collegead admin { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPostSend()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            var client = new CollegeAdmin
            {
                college_id = admin.clge_id,
                college_email = admin.clge_email,
                college_name = admin.clge_name,
                college_pass = admin.clge_pass,
                purchased = admin.purchase
            };
           
            client.subscription = null;
            
            _context.collegeadmins.Add(client);
            _context.SaveChanges();
            return RedirectToPage("/SuperAdmin/Dashboard");
            
        }
    }
}
