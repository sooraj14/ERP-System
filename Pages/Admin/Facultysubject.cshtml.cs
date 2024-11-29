using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Runtime.Intrinsics.X86;

namespace ERP_System.Pages.Admin
{
    public class FacultysubjectModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Facultymappingsubject fms { get; set; }
        

        public FacultysubjectModel(ApplicationDbContext context)
        {
            _context = context;

        }

        public List<FacultyjoinSubject> facultyjoinSubjects { get; set; } = new List<FacultyjoinSubject>();
     
        public List<Data.Entity.Faculty> facsubj { get; set; } = new List<Data.Entity.Faculty>();

        public List<Streams> stream { get; set; } = new List<Streams>();

        public List<Subject> sub { get; set; } = new List<Subject>();
        public List<CollegeAdmin> clgadmins { get; set; } = new List<CollegeAdmin>();
        public IActionResult OnGet()
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            if (college_id == null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
            var admin = _context.collegeadmins.FirstOrDefault(ca => ca.college_id == college_id);
            var streamavailable = _context.streamdetails.Where(sd => sd.college_id == college_id).ToList();
            var facultyinfo = _context.facultydetails.Where(sd => sd.college_id == college_id).ToList();

            if (facultyinfo != null && facultyinfo.Any())
            {
                facsubj.AddRange(facultyinfo);
            }

            if (streamavailable != null && streamavailable.Any())
            {
                stream.AddRange(streamavailable);

            }


            var subdata = _context.subjects.Where(sd => sd.college_id == college_id).ToList();
            if (subdata != null && subdata.Any())
            {
                sub.AddRange(subdata);
            }


            if (admin != null)
            {
                clgadmins.Add(admin);

            }


            facultyjoinSubjects = (from FacultySubject in _context.facultysubjects
                                  join Subject in _context.subjects
                                  on FacultySubject.sub_id    equals Subject.sub_id
                                  join Streams in _context.streamdetails
                                  on FacultySubject.stream_id equals Streams.stream_id
                                  join Faculty in _context.facultydetails
                                  on FacultySubject.fac_id equals Faculty.fac_id
                                  select new FacultyjoinSubject
                                  {
                                      FacultyName = Faculty.fac_name,
                                      SubjectName = Subject.sub_name,
                                      Department = Streams.stream_name,
                                      Semesters = Subject.sem_id,
                                      stream_id = Subject.stream_id
                                  }).ToList();


                       return Page();
        }

        public IActionResult OnPostFacultyMapSubject()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }



            int? college_id = HttpContext.Session.GetInt32("college_id");
            if (college_id == null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
            var facultymapsubj = new FacultySubject
            {
                fac_id = fms.fac_id,
                sub_id = fms.sub_id,
                college_id = (int)college_id,
                stream_id = fms.stream_id,
                sem_id = fms.sem_id,

            };

            _context.facultysubjects.Add(facultymapsubj);
            _context.SaveChanges();
            Console.WriteLine("data saved");
           
            return RedirectToPage();
        }

    }
}
