using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClass;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Admin
{
  
    public class AddNoticeModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Streams> stream { get; set; } = new List<Streams>();

        [BindProperty]
        public AdminNoticecs an {  get; set; }
        public AddNoticeModel(ApplicationDbContext dbcontext)
        {
           _context = dbcontext;
        }
        public IActionResult OnGet()
        {

            int? college_id = HttpContext.Session.GetInt32("college_id");
            var streamavailable = _context.streamdetails.Where(sd => sd.college_id == college_id).ToList();

            
            if (streamavailable !=null)
            {
                stream.AddRange(streamavailable);
            }
            stream = _context.streamdetails
                .Where(sd => sd.college_id == college_id)
                .ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
                
            var addnoticetodb = new Notice
            {
                  
                  stream_id = an.stream_id,
                 college_id = (int)college_id,
                  notice_Data = an.notice_Data

            };
            _context.notices.Add(addnoticetodb);
            _context.SaveChanges();
            return RedirectToPage();
        }
    }
}
