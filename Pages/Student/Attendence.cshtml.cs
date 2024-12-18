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
        public List<Subj> sub { get; set; } = new List<Subj>();

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
                   on new { s.sub_id, s.sem_id }
                   equals new { a.sub_id, a.sem_id }
                   where a.student_id == id
                   select new Subj
                   {
                       sub_name = s.sub_name,
                       date = a.date,
                       is_active = a.Active


                   }).ToList();
          
           
            return Page();
        }
    }
}