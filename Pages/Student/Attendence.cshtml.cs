using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ERP_System.Pages.Student
{
    public class AttendenceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public AttendenceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Subj> sub{ get; set; } = new List<Subj>();

        [BindProperty]
        //public List<attendence> attend { get; set; } = new List<attendence>();
        public double percentage { get; set; }
        
        public IActionResult OnGet()
        {

            var total_attendence = _context.attences.Count();
            var present_attendence = _context.attences.Count(p => p.Active == true);

            if (total_attendence > 0)
            {
                percentage = (double)present_attendence / total_attendence * 100;
            }
            int? id = HttpContext.Session.GetInt32("stud_id");
            var store = _context.studentdetails.Find(id);

            if (id == null)
            {
                return Page()
;
            }

            sub = (from s in _context.subjects
                   join a in _context.attences
                   on s.sub_id equals a.sub_id into attendanceGroup
                   where s.stream_id == store.stream_id 
                   select new Subj
                   {
                       sub_id = s.sub_id,
                       stream_id = s.stream_id,
                       sem_id = s.sem_id,
                       fac_id = s.fac_id,
                       college_id = s.college_id,
                       sub_name = s.sub_name,
                       is_active = s.is_active,
                       AttendanceDetails = attendanceGroup.Where(a => a.student_id == id).Select(a => new Attendence
                       {
                           date = a.date,
                           Active = a.Active,
                           student_id = a.student_id
                       }).ToList()
                   }).ToList();
            return Page();
        }
    }
}
