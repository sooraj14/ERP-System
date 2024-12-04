using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Admin
{
    public class TimeTableModel : PageModel
    {

        [Authorize(Policy = "Standard Only")]
        public void OnGet()
        {
         
        }
    }
}
