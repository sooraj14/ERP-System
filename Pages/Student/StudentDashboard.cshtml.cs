using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace ERP_System.Pages.Student
{
    public class StudentDashboardModel : PageModel  
    {
        private readonly ApplicationDbContext _context;
        public StudentDashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<StudentProfile> user { get; set; } = new List<StudentProfile>();

        //viewdata purpose
        public string sname { get; set; }
        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("stud_id");
            if (id == null)
            {
                return RedirectToPage("/Student/StudentLogin");
            }

            var student = _context.studentdetails.FirstOrDefault(s=>s.student_id == id);
            if (student!=null)
            {
                sname = student.student_name;
                
            }

            user = (from s in _context.studentdetails
                    join c in _context.streamdetails
                    on s.stream_id equals c.stream_id
                    where s.student_id == id
                    select new StudentProfile
                    {
                        reg_number = s.reg_num,
                        name=s.student_name,
                        sem =s.sem_id,
                        branch = c.stream_name,
                        contact = s.contact_no

                    }).ToList();
           
            return Page();
        }
        
    }
}
