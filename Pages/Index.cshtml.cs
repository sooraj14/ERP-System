using ERP_System.Data.context;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Numerics;
using System.Security.Claims;

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

        public async Task<IActionResult> OnPost()
        {
            var collegeadmin = _context.collegeadmins.FirstOrDefault(c=>c.college_email==login.email && c.college_pass==login.password);
             HttpContext.Session.SetInt32("clge_admin", collegeadmin.college_id);

            if(collegeadmin.subscription == null)
            {
                return RedirectToPage("Plans");
            }   
          
            if (collegeadmin.Active== true)
            {
                HttpContext.Session.SetInt32("college_id", collegeadmin.college_id);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, collegeadmin.college_email),
            new Claim("CollegeId", collegeadmin.college_id.ToString()),
            new Claim("Subscription", collegeadmin.subscription)
        };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

             
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/Admin/EmptyPage");
               
            }
            TempData["loginfailed"] = "college Blocked";
            return Page();
        }
      
     }
}
