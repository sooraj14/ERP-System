
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ERP_System.Data.context;
using ERP_System.Models;
using System.Linq;
using ERP_System.Data.Entity;

namespace ERP_System.Pages.Faculty
{
    public class ViewOrEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewOrEditModel(ApplicationDbContext context)
        {
            _context = context;
        }


        public void OnGet()
        {

        }
    }
      
}