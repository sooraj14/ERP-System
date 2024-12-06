using ERP_System.Data.context;
using ERP_System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ERP_System.Pages.Faculty
{
    public class FacultyDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FacultyDashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string FacultyName { get; set; }
        public string CollegeName { get; set; }
        public List<Subject> AssignedSubjects { get; set; } = new List<Subject>();
        public List<Notice> Notices { get; set; } = new List<Notice>();

        public IActionResult OnGet()
        {
            int? facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/FacultyLogin");
            }

            var faculty = _context.facultydetails.FirstOrDefault(f => f.fac_id == facultyId);
            if (faculty != null)
            {
                FacultyName = faculty.fac_name;

                AssignedSubjects = _context.facultysubjects
                    .Where(fs => fs.fac_id == facultyId)
                    .Join(_context.subjects,
                          fs => fs.sub_id,
                          s => s.sub_id,
                          (fs, s) => s)
                    .Where(s => s.is_active)
                    .ToList();

                var college = _context.collegeadmins.FirstOrDefault(c => c.college_id == faculty.college_id);
                if (college != null)
                {
                    CollegeName = college.college_name;
                }

                Notices = _context.notices
                    .Where(n => n.college_id == faculty.college_id && n.stream_id == faculty.stream_id)
                    .Take(5)
                    .ToList();
            }
            else
            {
                return RedirectToPage("/Faculty/FacultyLogin");
            }
            return Page();
        }
    }
}