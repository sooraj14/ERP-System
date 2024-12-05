using ERP_System.Data.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Pages.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync ()
        {
            var menulist = await _context.features.Where(f => f.role=="SuperAdmin").ToListAsync();

            

            return View(menulist);
        }

       
    }
}
