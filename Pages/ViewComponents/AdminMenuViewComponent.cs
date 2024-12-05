using ERP_System.Data.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Pages.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public AdminMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var clgmenu = await _context.features.Where(fd => fd.role == "College Admin").ToListAsync();
            return View(clgmenu);
        }
    }
}
