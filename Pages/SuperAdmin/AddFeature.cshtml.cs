using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.SuperAdmin
{
    public class AddFeatureModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AddFeatureModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Dynamicfeature dynamicfeature { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var fea = new Features
            {
                fea_name = dynamicfeature.fea_name,
                path = dynamicfeature.path,
                role = dynamicfeature.role,
                plan_type = dynamicfeature.plan_type,
                active = dynamicfeature.active
            };
            _context.features.Add(fea);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
