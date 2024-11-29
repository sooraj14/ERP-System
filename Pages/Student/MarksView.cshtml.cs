using ERP_System.Data.context;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Student
{
    public class MarksViewModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public MarksViewModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Marksheet> Newuser { get; set; } = new List<Marksheet>();
        public List<Internalsheet> marks { get; set; } = new List<Internalsheet>();
        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("stud_id");
            if (id == null)
            {
                return Page();
            }
            Newuser = (from s in _context.studentdetails
                    join st in _context.streamdetails 
                    on s.stream_id equals st.stream_id
                   join c in _context.collegeadmins on s.college_id equals c.college_id
                    where s.student_id == id
                    select new Marksheet
                    {
                        reg_number = s.reg_num,
                        stuname = s.student_name,
                        semester = s.sem_id,
                        branchname = st.stream_name,
                        college_name = c.college_name
                    }).ToList();
             marks = (from s in _context.subjects
                         join i in _context.internals on s.sub_id equals i.sub_id into internalsGroup
                         from i in internalsGroup.DefaultIfEmpty() // Left join with internals
                         join m in _context.marks on s.sub_id equals m.sub_id into marksGroup
                         from m in marksGroup.DefaultIfEmpty() // Left join with marks
                         where i.student_id == id
                         select new Internalsheet
                         {
                             internal_type = i != null ? i.exm_type : null,  
                             internal_marks = i != null ? i.scored_marks : 0, 
                             external_marks = m != null ? m.sem_marks : 0,
                             subject_name = s.sub_name
                         }).ToList();

            return Page();
        }
    }
}
